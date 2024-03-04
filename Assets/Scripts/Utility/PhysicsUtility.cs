using UnityEngine;

public static class PhysicsUtility
{
    static readonly RaycastHit[] hitBuffer = new RaycastHit[2];


    public static bool RaycastIgnoreSelf(Vector3 origin, Vector3 direction, Transform self, out RaycastHit hit, float maxDistance = float.MaxValue)
    {
        hit = new RaycastHit();
        
        Ray ray = new Ray(origin, direction);
        int count = Physics.RaycastNonAlloc(ray, hitBuffer, maxDistance);

        if (count == 0)
        {
            return false;
        }
        else if (count == 1)
        {
            RaycastHit possibleHit = hitBuffer[0];
            
            if (possibleHit.transform != null && possibleHit.transform != self && !possibleHit.transform.IsChildOf(self))
            {
                hit = possibleHit;
                return true;
            }
        
            return false;
        }
        else // count == 2
        {
            // check if any of them is self
            for (int i = 0; i < 2; i++)
            {
                RaycastHit possibleHit = hitBuffer[i];
            
                if (possibleHit.transform == self || possibleHit.transform.IsChildOf(self))
                {
                    //Small math hack! Return another one if current is self
                    hit = hitBuffer[1 - i]; 
                    return true;
                }
            }

            int closestIndex;
            
            //check who is closer
            if (Vector3.SqrMagnitude(origin - hitBuffer[0].point) > Vector3.SqrMagnitude(origin - hitBuffer[1].point))
                closestIndex = 1;
            else
                closestIndex = 0;


            hit = hitBuffer[closestIndex];

            return true;
        }
    }
}
