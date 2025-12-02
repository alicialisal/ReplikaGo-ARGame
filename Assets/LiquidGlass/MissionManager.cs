using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public GameObject[] missionImages; // Assign MissionImage1-3
    public Button acceptButton;

    private int currentMission;
    public GameObject bottomSheetObject;
    public BottomSheetController bottomSheet;

    [SerializeField] GameObject tutorialManager; // Assign TutorialManager GameObject

    void Start()
    {
        if (PlayerPrefs.GetInt("tourDone", 0) == 0) {
            // buka tur dulu
            // ShowTutorial();
            HideMission();
            Debug.Log("Tour belum selesai.");
            return;
        }
        else {
            // jika tur sudah selesai, baru tampilkan misi
            Debug.Log("Tour sudah selesai. Memulai Misi");
            currentMission = PlayerPrefs.GetInt("currentMission", 0);
            ShowMission(currentMission);

            acceptButton.onClick.AddListener(OnAcceptMission);

            if (PlayerPrefs.GetInt("missionAccepted", 0) == 1)
            {
                // tutorialManager.SetActive(false);
                // Misi sedang berjalan, sembunyikan gambar misi dan tombol terima
                HideMission();
            }
        }
    }

    void HideMission()
    {
        foreach (var img in missionImages)
            img.SetActive(false);
        
        acceptButton.gameObject.SetActive(false);
    }

    void ShowMission(int index)
    {
        Debug.Log("Menampilkan misi ke-" + (index + 1));

        for (int i = 0; i < missionImages.Length; i++)
            missionImages[i].SetActive(i == index);
    }

    void OnAcceptMission()
    {
        // Hilangkan gambar misi
        missionImages[currentMission].SetActive(false);
        acceptButton.gameObject.SetActive(false);

        // Simpan kalau misi sedang berjalan
        PlayerPrefs.SetInt("missionAccepted", 1);
        PlayerPrefs.Save();

        bottomSheetObject.SetActive(true);
        // Tampilkan bottom sheet versi kecil (collapsed)
        bottomSheet.ShowCollapsed();

        // Pindah ke scene AR / gameplay
        // UnityEngine.SceneManagement.SceneManager.LoadScene("ARScene");
    }

    public void CompleteMission()
    {
        PlayerPrefs.SetInt("missionAccepted", 0);

        currentMission++;
        PlayerPrefs.SetInt("currentMission", currentMission);
        PlayerPrefs.Save();

        if (currentMission < missionImages.Length)
            ShowMission(currentMission);
    }
}
