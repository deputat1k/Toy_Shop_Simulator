using ToyShop.Core.Interfaces;
using UnityEngine;
using UnityEngine.Pool;

namespace ToyShop.Gameplay.NPC.Spawning
{
    public class NpcPool
    {
        private readonly NpcController _prefab;
        private readonly Transform _poolParent;
        private readonly ObjectPool<NpcController> _pool;

        public NpcPool(NpcController prefab, Transform poolParent, int defaultCapacity, int maxSize)
        {
            _prefab = prefab;
            _poolParent = poolParent;

            _pool = new ObjectPool<NpcController>(
                createFunc: CreateNpc,
                actionOnGet: OnGetFromPool,
                actionOnRelease: OnReleaseToPool,
                actionOnDestroy: OnDestroyPoolObject,
                collectionCheck: true,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize);
        }

        public NpcController Get() => _pool.Get();

        public void Release(NpcController npc) => _pool.Release(npc);

        public int CountActive => _pool.CountActive;

        private NpcController CreateNpc()
        {
            NpcController instance = Object.Instantiate(_prefab, _poolParent);
            instance.gameObject.SetActive(false);
            return instance;
        }

        private void OnGetFromPool(NpcController npc)
        {
            npc.gameObject.SetActive(true);
        }

        private void OnReleaseToPool(NpcController npc)
        {
            npc.ResetNpc();
            npc.gameObject.SetActive(false);
            npc.transform.SetParent(_poolParent);
        }

        private void OnDestroyPoolObject(NpcController npc)
        {
            if (npc != null)
                Object.Destroy(npc.gameObject);
        }
    }
}