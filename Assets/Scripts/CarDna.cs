using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDna
{
    private float[] genome;
    public float mutationRate = 0.1f;
    public float mutationStrength = 0.2f;

    public CarDna(int genomeSize)
    {
        genome = new float[genomeSize];
        randomizeGene();
    }
    public void setGenome(float[] genome)
    {
        this.genome = genome;
    }

    public void randomizeGene()
    {
        // 10 due to 2 ouputs, steer + throttle, 5 inputs due to sensors
        genome = new float[10];
        for (int i = 0; i < 10; i++)
        {
            float randomWeight = UnityEngine.Random.Range(-1f, 1f);
            genome[i] = randomWeight;
        }
    }

    public void Mutate()
    {
        for (int i = 0; i < genome.Length; i++)
        {
            if (UnityEngine.Random.value < mutationRate)
            {
                genome[i] += UnityEngine.Random.Range(-mutationStrength, mutationStrength);
            }
        }
    }

    public float[] getGenome()
    {
        return genome;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (genome == null || genome.Length == 0)
        {
            randomizeGene();
        }
    }
}
