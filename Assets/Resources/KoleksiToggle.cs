using UnityEngine;

public class KoleksiToggle : MonoBehaviour
{
    [SerializeField] private GameObject koleksiPanel; // Tarik di Inspector
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        koleksiPanel.SetActive(false);
    }

    // Update is called once per frame
    public void ShowKoleksi()
    {
        koleksiPanel.SetActive(!koleksiPanel.activeSelf);
    }
}
