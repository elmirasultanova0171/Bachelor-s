using System;
using System.Collections.Generic;


public class TemporalLocality{


    public bool sum(double[,] input, double[,] output, int size)
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                output[i, j] = input[j, i];
            }
        }
        return output[0, size - 1] != 0; // Assuming a boolean return value is based on this condition.
    }
    public bool sumOptmzd(double[,] input, double[,] output, int size)
    {
        const int TileSize = 16;

        for (int ii = 0; ii < size; ii += TileSize)
        {
            for (int jj = 0; jj < size; jj += TileSize)
            {
                for (int i = ii; i < Math.Min(ii + TileSize, size); i++)
                {
                    for (int j = jj; j < Math.Min(jj + TileSize, size); j++)
                    {
                        output[i, j] = input[j, i];
                    }
                }
            }
        }

        return output[0, size - 1] != 0; // Assuming a boolean return value is based on this condition.
    }

}
