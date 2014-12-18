using System;
using System.Collections.Generic;
using System.Linq;

public class CGaussian
{
    private bool uselast = true;
    private double next_gaussian = 0.0;
    private Random random = new Random();

    public double BoxMuller()
    {
        if (uselast)
        {
            uselast = false;
            return next_gaussian;
        }
        else
        {
            double v1, v2, s;
            do
            {
                v1 = 2.0 * random.NextDouble() - 1.0;
                v2 = 2.0 * random.NextDouble() - 1.0;
                s = v1 * v1 + v2 * v2;
            } while (s >= 1.0 || s == 0);

            s = System.Math.Sqrt((-2.0 * System.Math.Log(s)) / s);

            next_gaussian = v2 * s;
            uselast = true;
            return v1 * s;
        }
    }

    public double BoxMuller(double mean, double standard_deviation)
    {
        return mean + BoxMuller() * standard_deviation;
    }

    public int Next(int min, int max)
    {
        double deviations = 3.5;
        int r;
        while ((r = (int)BoxMuller(min + (max - min) / 2.0, (max - min) / 2.0 / deviations)) > max || r < min)
        {
        }

        return r;
    }
}
