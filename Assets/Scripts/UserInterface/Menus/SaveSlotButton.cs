using UnityEngine;
using TMPro;

public class SaveSlotButton : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI slotNumText;
    [SerializeField] private TextMeshProUGUI slotDifficultyText;
    [SerializeField] private TextMeshProUGUI slotTimeText;
    [SerializeField] private TextMeshProUGUI slotProgressText;

    [SerializeField] private int slotNumber;

    public void SelectSaveSlot() {
        MainMenuManager.Instance.SelectSaveSlot(slotNumber);
    }

    public void UpdateInformation () {
        if (SaveLoad.saveMetas.SavesDifficulty[slotNumber - 1] > 0) {
            slotNumText.text = "Slot Number " + slotNumber;
            switch(SaveLoad.saveMetas.SavesDifficulty[slotNumber - 1]) {
                case 1:
                    slotDifficultyText.text = "Difficulty: Easy";
                    break;
                case 2:
                    slotDifficultyText.text = "Difficulty: Normal";
                    break;
                case 3:
                    slotDifficultyText.text = "Difficulty: Hard";
                    break;
                default:
                    slotDifficultyText.text = "Difficulty: <color=red>Error</color>";
                    break;
            }

            int hours = SaveLoad.saveMetas.SavesPlayTime[slotNumber - 1] / 360;
            int minutes = SaveLoad.saveMetas.SavesPlayTime[slotNumber - 1] / 60;
            string minString;
            if (minutes < 10)
                minString = "0" + minutes;
            else
                minString = minutes.ToString();

            slotTimeText.text = "Time: " + hours + ":" + minString;
            slotProgressText.text = "Progress: " + SaveLoad.saveMetas.SavesProgress[slotNumber - 1] + "%";
        } else {
            slotNumText.text = "New Game";
            slotDifficultyText.text = "";
            slotTimeText.text = "";
            slotProgressText.text = "";
        }
    }
}
