using MechSquad.Battle;
using System.Collections.Generic;
using UnityEngine;

namespace MechSquad
{
	public class SeManager : MonoBehaviour
	{
		static SeManager _instance;
		public static SeManager Instance
		{
			get
			{
				if (_instance != null)
					return _instance;

				_instance = FindObjectOfType<SeManager>();
				if (_instance != null)
					return _instance;

				var go = new GameObject("SeManager");
				_instance = go.AddComponent<SeManager>();
				return _instance;
			}
		}

		private void Awake()
		{
			_instance = this;
		}

		Dictionary<string, List<SePlayer>> _playingSePlayers = new Dictionary<string, List<SePlayer>>();
		Dictionary<int, Queue<SePlayer>> _recycledSePlayers = new Dictionary<int, Queue<SePlayer>>();

		public SePlayer GetInstanceFromPrefab(SePlayer p, string sid = "")
		{
			var id = string.Format("{0}@{1}", p.GetInstanceID(), sid);

			var playingList = GetPlayingFromPool(id);
			if (playingList.Count < p.MultipleLimit)
			{
				var ret = GetOrCreateFromRecyclePool(p);
				return ret;
			}

			if (p.MultipleOverrideType == SeMultipleOverriadeTypeEnum.Cancel)
			{
				return null;
			}
			else if(p.MultipleOverrideType == SeMultipleOverriadeTypeEnum.Override)
			{
				return playingList[0];
			}
			else
			{
				throw new System.NotImplementedException();
			}
		}

		public List<SePlayer> GetPlayingFromPool(string id)
		{
			List<SePlayer> set;
			if (!_playingSePlayers.TryGetValue(id, out set))
			{
				set = new List<SePlayer>();
				_playingSePlayers.Add(id, set);
			}
			return set;
		}
		public void AddToPlayingPool(string id, SePlayer p)
		{
			List<SePlayer> set;
			if (!_playingSePlayers.TryGetValue(id, out set))
			{
				set = new List<SePlayer>();
				_playingSePlayers.Add(id, set);
			}
			set.Add(p);
		}

		public void RecycleInstance(SePlayer p, string sid = "")
		{
			var id = string.Format("{0}@{1}", p.PrefabInstanceID, sid);

			var playingList = GetPlayingFromPool(id);
			playingList.Remove(p);

			Queue<SePlayer> queue;
			if (!_recycledSePlayers.TryGetValue(p.PrefabInstanceID, out queue))
			{
				queue = new Queue<SePlayer>();
				_recycledSePlayers.Add(p.GetInstanceID(), queue);
			}
			queue.Enqueue(p);
		}

		SePlayer GetOrCreateFromRecyclePool(SePlayer p)
		{
			Queue<SePlayer> queue;
			if (!_recycledSePlayers.TryGetValue(p.GetInstanceID(), out queue))
			{
				queue = new Queue<SePlayer>();
				_recycledSePlayers.Add(p.GetInstanceID(), queue);
			}

			if (queue.Count != 0)
			{
				return queue.Dequeue();
			}

			var ret = Instantiate(p);
			ret.name = p.name;
			ret.PrefabInstanceID = p.GetInstanceID();
			SetInstanceToChild(ret.transform, p.name);
			return ret;
		}

		void SetInstanceToChild(Transform instance, string parentName)
		{
			if (instance.transform.parent == null || instance.transform.parent.name == parentName)
			{
				var parentTransform = transform.Find(parentName);
				if (parentTransform == null)
				{
					parentTransform = new GameObject(parentName).transform;
					parentTransform.SetParent(transform);
				}
				instance.transform.SetParent(parentTransform);
			}
		}
	}
}
