using UnityEngine;

public static class Extension
{
    private static float _xBorder, _yBorder;
    private static float _distanceFromPlayer = 4f;

    public static void SetRandomPosition(this Transform transform, float xBorder, float yBorder)
    {
        _xBorder = xBorder;
        _yBorder = yBorder;

        var x = 
            Random.Range(Random.Range(-_xBorder, -_distanceFromPlayer),
            Random.Range(_distanceFromPlayer, _xBorder));
        var y = 
            Random.Range(Random.Range(-_yBorder, -_distanceFromPlayer),
            Random.Range(_distanceFromPlayer, _yBorder));

        transform.position = new Vector3(x, y, 0);
    }

    public static void CheckBorder(this Transform targetTransform)
    {
        //Check X Pos
        if (targetTransform.position.x > _xBorder)
        {
            targetTransform.position = new Vector2(-_xBorder, targetTransform.position.y);
        }
        else if (targetTransform.position.x < -_xBorder)
        {
            targetTransform.position = new Vector2(_xBorder, targetTransform.position.y);
        }

        //Check Y pos
        if (targetTransform.position.y > _yBorder)
        {
            targetTransform.position = new Vector2(targetTransform.position.x, -_yBorder);
        }
        else if (targetTransform.position.y < -_yBorder)
        {
            targetTransform.position = new Vector2(targetTransform.position.x, _yBorder);
        }
    }
}