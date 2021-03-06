﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Vixen.Cache.Sequence;
using Vixen.Commands;
using Vixen.Module;
using Vixen.Module.Controller;
using Vixen.Module.Timing;
using Vixen.Module.App;
using Vixen.Services;
using Vixen.Execution;
using Vixen.Execution.Context;
using Vixen.Factory;
using Vixen.Sys;
using Vixen.Sys.Output;
using NLog;

namespace Vixen.Export
{
    public enum ExportNotifyType
    {
        NETSAVE,
        LOADING,
        EXPORTING,
        SAVING,
        COMPLETE
    };

    public class Export
    {
        Guid _controllerTypeId = new Guid("{F79764D7-5153-41C6-913C-2321BC2E1819}");
        List<OutputController> _nonExportControllers = null;

        private IExportWriter _output;
        private Dictionary<string, IExportWriter> _writers = null;
        private Dictionary<string, string> _exportFileTypes = null;

        private bool _exporting = false;
        private bool _cancelling = false;
        private string _exportDir = null;
        PreCachingSequenceEngine _preCachingSequenceEngine = null;
        private ExportCommandHandler _exporterCommandHandler = null;
        private List<byte> _eventData = null;

        public delegate void SequenceEventHandler(ExportNotifyType notify);
        public event SequenceEventHandler SequenceNotify;

        #region Contructor
        public Export()
        {
            _exportFileTypes = new Dictionary<string, string>();
            _writers = new Dictionary<string, IExportWriter>();
            var type = typeof(IExportWriter);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.Equals(type));

            IExportWriter exportWriter;
            foreach (Type theType in types.ToArray())
            {
                exportWriter = (IExportWriter)Activator.CreateInstance(theType);
                _writers[exportWriter.FileType] = exportWriter;
                _exportFileTypes[exportWriter.FileTypeDescr] = exportWriter.FileType;
            }

            UpdateInterval = VixenSystem.DefaultUpdateInterval;  //Default the UpdateInterval to the global interval

            _eventData = new List<byte>();
            _exporterCommandHandler = new ExportCommandHandler();

            _exporting = false;
            _cancelling = false;

            SavePosition = 0;
        }
        #endregion

        #region Properties
        public string ExportDir
        {
            get
            {
                if (_exportDir == null)
                {
                    ExportDir = Path.Combine(Paths.DataRootPath, "Export");
                }
                return _exportDir;
            }

            set
            {
                _exportDir = value;
                checkExportdir();
            }
        }

        public string[] FormatTypes
        {
            get
            {
                return _exportFileTypes.Keys.ToArray();
            }
        }

        public string OutFileName { get; set; }

        public int UpdateInterval { get; set; }

        public Dictionary<string, string> ExportFileTypes
        {
            get
            {
                return _exportFileTypes;
            }
        }

        private List<OutputController> NonExportControllers
        {
            get
            {
                if (_nonExportControllers == null)
                {
                    _nonExportControllers = VixenSystem.OutputControllers.ToList().FindAll(x => x.ModuleId != _controllerTypeId);
                }
                return _nonExportControllers;
            }
        }

        public TimeSpan ExportPosition
        {
            get
            {
                if (_preCachingSequenceEngine != null)
                {
                    return _preCachingSequenceEngine.Position;
                }
                else
                {
                    return new TimeSpan(0);
                }
            }
        }

        public decimal SavePosition { get; set; }

        public List<ControllerExportInfo> ControllerExportData
        {
            get
            {
                int index = 0;
                List<ControllerExportInfo> retVal = new List<ControllerExportInfo>();
                NonExportControllers.ForEach(x => retVal.Add(new ControllerExportInfo(x, index++)));
                return retVal;
            }
        }

        #endregion

        #region Operational
        private bool checkExportdir()
        {
            if (!Directory.Exists(_exportDir))
            {
                try
                {
                    Directory.CreateDirectory(_exportDir);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        private OutputController FindExportControler()
        {
            return
                VixenSystem.OutputControllers.ToList().Find(x => x.ModuleId.Equals(_controllerTypeId));
        }

        public void WriteControllerInfo(ISequence sequence)
        {
            int chanStart = 1;

            string xmlOutName =
                Path.GetDirectoryName(OutFileName) +
                Path.DirectorySeparatorChar +
                Path.GetFileNameWithoutExtension(OutFileName) +
                "_Network.xml";

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            using (XmlWriter writer = XmlWriter.Create(xmlOutName, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Vixen3_Export");
                writer.WriteElementString("Resolution", UpdateInterval.ToString());
                writer.WriteElementString("OutFile", Path.GetFileName(OutFileName));
                writer.WriteElementString("Duration", sequence.Length.ToString());

                writer.WriteStartElement("Network");
                foreach (ControllerExportInfo exportInfo in ControllerExportData)
                {
                    writer.WriteStartElement("Controller");
                    writer.WriteElementString("Index", exportInfo.Index.ToString());
                    writer.WriteElementString("Name", exportInfo.Name);
                    writer.WriteElementString("StartChan", chanStart.ToString());
                    writer.WriteElementString("Channels", exportInfo.Channels.ToString());
                    writer.WriteEndElement();

                    chanStart += exportInfo.Channels;
                }
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

        }

        public void DoExport(ISequence sequence, string outFormat)
        {
            string fileType;

            _exporting = true;
            _cancelling = false;

            if ((sequence != null) && (_exportFileTypes.TryGetValue(outFormat,out fileType)))
            {
                if (_writers.TryGetValue(fileType, out _output))
                {
                    _preCachingSequenceEngine = new PreCachingSequenceEngine(UpdateInterval);
                    _preCachingSequenceEngine.Sequence = sequence;
                    _preCachingSequenceEngine.SequenceCacheEnded += SequenceCacheEnded;
                    _preCachingSequenceEngine.SequenceCacheStarted += SequenceCacheStarted;
                    _preCachingSequenceEngine.Start();

                    SequenceNotify(ExportNotifyType.EXPORTING);
                    WriteControllerInfo(sequence);
                }
            }
        }
        
        public void Cancel()
        {
            _cancelling = true;
            _preCachingSequenceEngine.Stop();
        }

        private void UpdateState(ICommand[] outputStates)
        {
            _eventData.Clear();

            for (int i = 0; i < outputStates.Length; i++)
            {
                _exporterCommandHandler.Reset();
                ICommand command = outputStates[i];
                if (command != null)
                {
                    command.Dispatch(_exporterCommandHandler);
                }
                _eventData.Add(_exporterCommandHandler.Value);
            }
            _output.WriteNextPeriodData(_eventData);
        }
        #endregion

        #region Events
        void SequenceCacheStarted(object sender, Vixen.Cache.Event.CacheStartedEventArgs e)
        {
            SavePosition = 0;
            if (SequenceNotify != null)
            {
                SequenceNotify(ExportNotifyType.LOADING);
            }
        }


        void SequenceCacheEnded(object sender, Vixen.Cache.Event.CacheEventArgs e)
        {
            if (_exporting == true)
            {                
                List<ICommand> commandList = new List<ICommand>();
                OutputStateListAggregator outAggregator = _preCachingSequenceEngine.Cache.OutputStateListAggregator;
                IEnumerable<Guid> outIds = outAggregator.GetOutputIds();
                int periods = outAggregator.GetCommandsForOutput(outIds.First()).Count();

                if (_cancelling == false)
                {
                    SequenceNotify(ExportNotifyType.SAVING);
                    _output.OpenSession(OutFileName, periods, outIds.Count());
                    for (int j = 0; j < periods; j++)
                    {
                        SavePosition = Decimal.Round(((Decimal)j / (Decimal)periods) * 100,2);
                        commandList.Clear();
                        foreach (Guid guid in outIds)
                        {
                            commandList.Add(outAggregator.GetCommandsForOutput(guid).ElementAt(j));
                        }
                        UpdateState(commandList.ToArray());
                    }

                    _output.CloseSession();

                    _preCachingSequenceEngine.SequenceCacheEnded -= SequenceCacheEnded;
                    _preCachingSequenceEngine.SequenceCacheStarted -= SequenceCacheStarted;
                }

                if (SequenceNotify != null)
                {
                    SequenceNotify(ExportNotifyType.COMPLETE);
                }
            }
        }
        
        #endregion

    }

    public class ControllerExportInfo
    {
        public ControllerExportInfo(OutputController controller, int index)
        {
            Name = controller.Name;
            Index = index;
            Channels = controller.OutputCount;
        }

        public int Index { get; set; }
        public int Channels { get; set; }
        public string Name { get; set; }
    }
}