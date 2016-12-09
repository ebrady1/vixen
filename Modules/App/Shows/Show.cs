﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Vixen.Module;
using System;
using Vixen.Services;
using Vixen.Sys;

namespace VixenModules.App.Shows
{
	[DataContract,
	Serializable]
	public class Show
	{
		protected List<ShowItem> _items;
		protected Guid id = Guid.Empty;

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public Guid ID
		{
			get
			{
				if (id == Guid.Empty) id = Guid.NewGuid();
				return id;
			}
			set
			{
				id = value;
			}
		}

		[DataMember]
		public List<ShowItem> Items
		{
			get
			{
				_items = _items != null ? _items : _items = new List<ShowItem>();

				// Set the GUID just in case!
				foreach (ShowItem item in _items)
				{
					item.CurrentShowID = ID;
				}
				return _items;
			}
			set
			{
				_items = value;
			}
		}

		public void LoadShowSequencesIntoShowItems(ShowItemType type)
		{
			//Ensure we only have one copy of each sequence
			var sequenceFileNames = new Dictionary<string, ISequence>();
			foreach (ShowItem item in GetItems(type))
			{
				if (item.Sequence_FileName != null)
				{
					if (sequenceFileNames.ContainsKey(item.Sequence_FileName))
					{
						item.Sequence = sequenceFileNames[item.Sequence_FileName];
					}
					else
					{
						var sequence = SequenceService.Instance.Load(item.Sequence_FileName);
						sequenceFileNames[item.Sequence_FileName] = sequence;
						item.Sequence = sequence;
					}
				}
			}

		}

		public List<ShowItem> GetItems(ShowItemType type)
		{
			List<ShowItem> items = new List<ShowItem>();

			foreach (ShowItem item in Items)
			{
				if (type == item.ItemType || type == ShowItemType.All)
				{
					items.Add(item);
				}
			}
			return items;
		}

		public void ReleaseAllActions()
		{
			foreach (ShowItem item in Items)
			{
				item.ClearAction();
			}
		}

		public ShowItem AddItem(ShowItemType itemType, string itemName)
		{
			ShowItem showItem = new ShowItem(itemType, itemName, this.ID);
			Items.Add(showItem);
			return showItem;
		}

		public void DeleteItem(ShowItem showItem)
		{
			if (Items.Contains(showItem))
				Items.Remove(showItem);
		}

		public object Clone()
		{
			return ObjectCopier.Clone(this);
		}

	}
}
