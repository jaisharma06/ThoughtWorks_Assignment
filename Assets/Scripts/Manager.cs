using UnityEngine;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    public Color lightTankColor;
    public Color heavyTankColor;

    [Header("Range")]
    public int minX;
    public int maxX;
    public int minY;
    public int maxY;

    public Tank lightTankPrefab;
    public Tank heavyTankPrefab;

    public int heavyTankCount;
    public int lightTankCount;
    public Transform tanksParent;

    void Start()
    {
        SpawnTanks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnTanks()
    {
        for (int i = 0; i < heavyTankCount; i++)
        {
            Tank t = Instantiate(heavyTankPrefab, tanksParent);
            int x = Random.Range(minX, maxX + 1);
            int y = Random.Range(minY, maxY + 1);

            t.transform.position = new Vector3(x, t.transform.position.y, y);
        }

        for (int i = 0; i < lightTankCount; i++)
        {
            Tank t = Instantiate(lightTankPrefab, tanksParent);

            int x = Random.Range(minX, maxX + 1);
            int y = Random.Range(minY, maxY + 1);

            t.transform.position = new Vector3(x, t.transform.position.y, y);
        }
    }
}
