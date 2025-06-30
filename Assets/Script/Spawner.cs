using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] foodPrefabs;
    [SerializeField] private float spawnDelay = 3f; // Thời gian giữa mỗi lần spawn
    [SerializeField] private float destroyDelay = 5f; // Thời gian trước khi hủy vật thể
    [SerializeField] private int spawnLimit = 30;
    [SerializeField] private int spawnCount = 0;
    [SerializeField] private float winPanelDelay = 15f;

    private void Start()
    {
        StartCoroutine(FoodSpawn());
    }

    private IEnumerator FoodSpawn()
    {
        while (spawnCount < spawnLimit)
        {
            // Chọn ngẫu nhiên một prefab
            var foodPrefabNumber = Random.Range(0, foodPrefabs.Length);
            var foodPrefab = foodPrefabs[foodPrefabNumber];

            // Spawn và lưu vào biến để Destroy sau 5 giây
            GameObject newFood = Instantiate(foodPrefab, transform.position, foodPrefab.transform.rotation);
            Destroy(newFood, destroyDelay); // Hủy sau 5 giây

            spawnCount++;

            yield return new WaitForSeconds(spawnDelay);
        }

        OnFinishSpawning();
    }

    private void OnFinishSpawning()
    {
        StartCoroutine(DelayFinish());
    }

    private IEnumerator DelayFinish()
    {
        yield return new WaitForSeconds(winPanelDelay);
        FinishLevel();
    }

    private void FinishLevel()
    {
        Debug.Log("Hoàn thành level!");

        // Gọi GameManager để hiện WinPanel và dừng game
        if (GameManager.Instance != null)
        {
            GameManager.Instance.FinishLevel();
        }
        else
        {
            Debug.LogWarning("Không tìm thấy GameManager Instance!");
        }
    }
}
