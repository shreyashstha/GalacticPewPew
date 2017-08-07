using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationCalculator{

    public FormationCalculator()
    {

    }

    /// <summary>
    /// Return a list of Vector3s that form a Grid formation. Like a chess board.
    /// </summary>
    /// <param name="xDimention">int x dimention of the formation</param>
    /// <param name="yDimention">int y dimention of the formation</param>
    /// <returns></returns>
    public LinkedList<Vector3> CrossGridFormation(int xDimention, int yDimention)
    {
        LinkedList<Vector3> FormationList = new LinkedList<Vector3>();
        bool even = true;
        float xPos = 0f;
        float yPos = 0f;

        for (int i = 0; i < yDimention; i++)
        {
            if (i % 2 == 0) even = true;
            else even = false;

            yPos = (float)(0 - (yDimention * 0.5) + (0.5 + (1 * i)));

            for (int j = 0; j < xDimention; j++)
            {
                if (even)
                {
                    if (j % 2 == 1)
                    {
                        xPos = (float)(0 - (xDimention * 0.5) + (0.5 + (1 * j)));
                        FormationList.AddLast(new Vector3(xPos, yPos, 0));
                    }
                }
                else
                {
                    if (j % 2 == 0)
                    {
                        xPos = (float)(0 - (xDimention * 0.5) + (0.5 + (1 * j)));
                        FormationList.AddLast(new Vector3(xPos, yPos, 0));
                    }
                }
            }
        }

        return FormationList;
    }

}
