using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generationTimer : MonoBehaviour
{
    public float timer = 0;
    public float trialTime = 15f;
    public int nextGenCount = 10;
    public GameObject carPrefab;
    public Vector3 startPosition = Vector3.zero;
    private List<GameObject> cars;

    void Start()
    {
        cars = new List<GameObject>();
        for (int i = 0; i < nextGenCount; i++)
        {
            // Offset each car slightly so they don't overlap
            Vector3 spawnPos = startPosition + new Vector3(2 * i, 2, 0);

            // Instantiate car prefab
            GameObject carObj = Instantiate(carPrefab, spawnPos, Quaternion.identity);
            carController agent = carObj.GetComponent<carController>();

            // Create random DNA for first generation
            agent.thisCarDna = new CarDna(10);
            cars.Add(carObj);
        }
    }
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= trialTime)
        {
            EndGeneration();
            timer = 0;
        }
    }
    public void EndGeneration()
    {
        float[] bestGene = getBestcar();
        foreach (var car in cars)
        {
            Destroy(car);
        }
        cars.Clear();
        StartCoroutine(SpawnNextGeneration(bestGene));
    }

    private IEnumerator SpawnNextGeneration(float[] bestGene)
    {
        yield return null; // wait one frame

        for (int i = 0; i < nextGenCount - 1; i++)
        {
            Vector3 spawnPos = startPosition + new Vector3(2 * i, 2, 0);
            GameObject carObj = Instantiate(carPrefab, spawnPos, Quaternion.identity);

            var agent = carObj.GetComponent<carController>();
            CarDna child = new CarDna(10);
            child.setGenome(bestGene);
            child.Mutate();
            agent.thisCarDna = child;

            cars.Add(carObj);
        }

        GameObject car = Instantiate(carPrefab, startPosition + new Vector3(0, 2, 0), Quaternion.identity);

        var carController = car.GetComponent<carController>();
        CarDna children = new CarDna(10);
        children.setGenome(bestGene);
        carController.thisCarDna = children;

        cars.Add(car);
    }
    public float[] getBestcar()
    {
        float[] bestGene = new float[10];
        float bestFitness = 0;

        foreach (var car in cars)
        {
            float carFitness = car.GetComponent<carController>().fitness;
            if (carFitness > bestFitness)
            {
                bestFitness = carFitness;
                bestGene = car.GetComponent<carController>().thisCarDna.getGenome();
            }
        }

        return bestGene;
    }
}

