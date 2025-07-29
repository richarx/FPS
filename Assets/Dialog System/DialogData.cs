using UnityEngine;

namespace Dialog_System
{
    [CreateAssetMenu(fileName = "DialogData", menuName = "ScriptableObjects/DialogData")]
    public class DialogData : ScriptableObject
    {
        public string npcName;
        [TextArea(1, 5)] public string[] dialoguesLines;
    }
}
