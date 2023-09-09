using System.Collections.Generic;
using Core.Common.SerializedCollections;
using Game.SlotsLogic;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "game_scriptable_object", menuName = "Game/Reference", order = 1)]
    public class GameScriptableObject : ScriptableObject, IScriptableObjectInstaller
    {
        [SerializeField] private SerializedDictionary<SlotItems, Sprite> _slotItems;
        public Dictionary<SlotItems, Sprite> SlotItems { private set; get; }
        
        public SlotItems[] SlotItemsArray { private set; get; }
        public Sprite[] SlotItemsSpriteArray { private set; get; }

        public void Init()
        {
            SlotItems = _slotItems.ToDictionary();
            
            SlotItemsArray = _slotItems.GetKeys().ToArray();
            SlotItemsSpriteArray = _slotItems.GetValues().ToArray();
        }
    }
}