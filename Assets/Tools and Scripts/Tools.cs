using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Bounds2
{
    public Vector2 position;
    public Vector2 size;

    public Vector2 bottomLeft => ComputeBottomLeft();
    public Vector2 topRight => ComputeTopRight();

    public float area => ComputeArea();

    private Vector2 ComputeTopRight()
    {
        return position + (Vector2.right * (size.x / 2.0f)) + (Vector2.up * (size.y / 2.0f));
    }

    private Vector2 ComputeBottomLeft()
    {
        return position + (Vector2.left * (size.x / 2.0f)) + (Vector2.down * (size.y / 2.0f));
    }

    private float ComputeArea()
    {
        return size.x * size.y;
    }

    public Bounds2(Vector2 Position, Vector2 Size)
    {
        position = Position;
        size = Size;
    }

    public static Bounds2 Extrude(Bounds2 targetBounds2, float f)
    {
        Vector2 newSize = targetBounds2.size + new Vector2(f * 2.0f, f * 2.0f);
        return new Bounds2(targetBounds2.position, newSize);
    }
    
    public static Vector2 ConstraintBounds(Bounds2 inBound, Bounds2 outBound)
    {
        float rightConstraint = (outBound.position.x + (outBound.size.x / 2.0f)) - (inBound.position.x + (inBound.size.x / 2.0f));
        float leftConstraint =  (outBound.position.x - (outBound.size.x / 2.0f)) - (inBound.position.x - (inBound.size.x / 2.0f));
        
        float upConstraint = (outBound.position.y + (outBound.size.y / 2.0f)) - (inBound.position.y + (inBound.size.y / 2.0f));
        float downConstraint = (outBound.position.y - (outBound.size.y / 2.0f)) - (inBound.position.y - (inBound.size.y / 2.0f));

        Vector2 direction = Vector2.zero;
        
        if (rightConstraint < 0)
            direction.x -= rightConstraint;

        if (leftConstraint > 0)
            direction.x -= leftConstraint;
        
        if (upConstraint < 0)
            direction.y -= upConstraint;

        if (downConstraint > 0)
            direction.y -= downConstraint;
        
        return direction;
    }
}

public static class Tools
{ 
    public static Vector3 ToVector3(this Vector2 vector, float z = 0.0f)
    {
        return new Vector3(vector.x, vector.y, z);
    }
    
    public static Vector2 ToVector2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    public static float Distance(this Vector2 position, Vector2 other)
    {
        return (other - position).magnitude;
    }

    public static Quaternion ToRotation(this Vector2 direction)
    {
        return Quaternion.AngleAxis(DirectionToDegree(direction), Vector3.forward);
    }

    public static Vector2 AddAngleToDirection(this Vector2 direction, float angle)
    {
        float directionAngle = DirectionToDegree(direction);
        float newAngle = directionAngle + angle;
        return DegreeToVector2(newAngle).normalized;
    }
    
    public static Vector2 AddRandomAngleToDirection(this Vector2 direction, float minInclusive, float maxInclusive)
    {
        float directionAngle = DirectionToDegree(direction);
        float newAngle = directionAngle + Random.Range(minInclusive, maxInclusive);
        return DegreeToVector2(newAngle).normalized;
    }

    public static float DirectionToDegree(Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
    
    public static float ToDegree(this Vector2 direction)
    {
        return DirectionToDegree(direction);
    }

    public static Vector2 RadianToVector2(float radian, float length = 1.0f)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)).normalized * length;
    }

    public static Vector2 DegreeToVector2(float degree, float length = 1.0f)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad).normalized * length;
    }
    
    public static float DegreeToRadian(float degrees)
    {
        return (Mathf.PI / 180.0f) * degrees;
    }

    public static float RandomPositiveOrNegative(float number = 1.0f)
    {
        int random = (Random.Range(0, 2) * 2) - 1;
        return random * number;
    }

    public static bool RandomBool()
    {
        return RandomPositiveOrNegative() > 0;
    }

    public static Vector2 RandomPositionInRange(Vector2 position, float range)
    {
        return position + (Random.insideUnitCircle * range);
    }
    
    public static Vector2 RandomPositionAtRange(Vector2 position, float range)
    {
        return position + (Random.insideUnitCircle * range);
    }
    
    public static List<T> GetRandomElements<T>(this IEnumerable<T> list, int elementsCount = 1)
    {
        return list.OrderBy(arg => Guid.NewGuid()).Take(elementsCount).ToList();
    }

    public static float NormalizeValue(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }
    
    public static float NormalizeValueInRange(float value, float min, float max, float rangeMin, float rangeMax)
    {
        return ((rangeMax - rangeMin) * ((value - min) / (max - min))) + rangeMin;
    }

    public static void DrawSquare(Bounds2 bound, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(bound.bottomLeft, bound.bottomLeft + (Vector2.up * bound.size.y));
        Gizmos.DrawLine(bound.bottomLeft, bound.bottomLeft + (Vector2.right * bound.size.x));
        Gizmos.DrawLine(bound.topRight, bound.topRight + (Vector2.down * bound.size.y));
        Gizmos.DrawLine(bound.topRight, bound.topRight + (Vector2.left * bound.size.x));
    }
    
    public static void DrawCone(Vector2 position, Vector2 direction, float angle, float distance, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(position, position + (direction.AddAngleToDirection(angle) * distance));
        Gizmos.DrawLine(position, position + (direction.AddAngleToDirection(-angle) * distance));
    }

    public static IEnumerator Fade(SpriteRenderer sprite, float duration, bool fadeIn, float delay = -1.0f)
    {
        if (delay > 0.0f)
            yield return new WaitForSeconds(delay);
        
        float fade = fadeIn ? 0.0f : 1.0f;
        float timer = duration;
        float increment = 1.0f / timer;
        Color color = sprite.color;
        
        while (timer > 0.0f)
        {
            color.a = fade;
            sprite.color = color;
            
            float delta = Time.deltaTime;
            fade += fadeIn ? delta * increment : -delta * increment;
            timer -= delta;
            
            yield return null;
        }
        
        color.a = fadeIn ? 1.0f : 0.0f;
        sprite.color = color;
    }
    
    public static IEnumerator Fade(LineRenderer line, float duration, bool fadeIn)
    {
        float fade = fadeIn ? 0.0f : 1.0f;
        float timer = duration;
        float increment = 1.0f / timer;
        Color color = line.startColor;
        
        while (timer > 0.0f)
        {
            color.a = fade;
            line.startColor = color;
            line.endColor = color;
            
            float delta = Time.deltaTime;
            fade += fadeIn ? delta * increment : -delta * increment;
            timer -= delta;
            
            yield return null;
        }
        
        color.a = fadeIn ? 1.0f : 0.0f;
        line.startColor = color;
        line.endColor = color;
    }
    
    public static IEnumerator Fade(Image sprite, float duration, bool fadeIn, float maxFade = 1.0f, bool scaledTime = true)
    {
        float fade = fadeIn ? 0.0f : maxFade;
        float timer = duration;
        float increment = maxFade / timer;
        Color color = sprite.color;
        
        sprite.gameObject.SetActive(true);
        
        while (timer > 0.0f)
        {
            color.a = fade;
            sprite.color = color;
            
            float delta = scaledTime ? Time.deltaTime : Time.unscaledDeltaTime;
            fade += fadeIn ? delta * increment : -delta * increment;
            timer -= delta;
            
            yield return null;
        }
        
        color.a = fadeIn ? maxFade : 0.0f;
        sprite.color = color;
        
        if (!fadeIn)
            sprite.gameObject.SetActive(false);
    }
    
    public static IEnumerator Fade(Light2D light, float duration, bool fadeIn, float maxFade = 1.0f, bool scaledTime = true)
    {
        float fade = fadeIn ? 0.0f : maxFade;
        float timer = duration;
        float increment = maxFade / timer;

        light.gameObject.SetActive(true);
        
        while (timer > 0.0f)
        {
            light.intensity = fade;
            
            float delta = scaledTime ? Time.deltaTime : Time.unscaledDeltaTime;
            fade += fadeIn ? delta * increment : -delta * increment;
            timer -= delta;
            
            yield return null;
        }
        
        light.intensity = fadeIn ? maxFade : 0.0f;;
        
        if (!fadeIn)
            light.gameObject.SetActive(false);
    }
    
    public static IEnumerator Fade(RawImage sprite, float duration, bool fadeIn, float maxFade = 1.0f)
    {
        float fade = fadeIn ? 0.0f : maxFade;
        float timer = duration;
        float increment = maxFade / timer;
        Color color = sprite.color;
        
        while (timer > 0.0f)
        {
            color.a = fade;
            sprite.color = color;
            
            float delta = Time.deltaTime;
            fade += fadeIn ? delta * increment : -delta * increment;
            timer -= delta;
            
            yield return null;
        }
        
        color.a = fadeIn ? maxFade : 0.0f;
        sprite.color = color;
        
        if (!fadeIn)
            sprite.gameObject.SetActive(false);
    }
    
    public static IEnumerator Fade(TextMeshPro text, float duration, bool fadeIn)
    {
        if (duration == 0.0f)
        {
            Color tmp = text.color;
            tmp.a = fadeIn ? 1.0f : 0.0f;
            text.color = tmp;
            yield break;
        }

        float fade = fadeIn ? 0.0f : 1.0f;
        float timer = duration;
        float increment = 1.0f / timer;
        Color color = text.color;
        
        while (timer > 0.0f)
        {
            color.a = fade;
            text.color = color;
            
            float delta = Time.deltaTime;
            fade += fadeIn ? delta * increment : -delta * increment;
            timer -= delta;
            
            yield return null;
        }
        
        color.a = fadeIn ? 1.0f : 0.0f;
        text.color = color;
    }

    public static IEnumerator FadeVolume(AudioSource source, float duration)
    {
        float startingVolume = source.volume;
        float timer = duration;

        while (timer > 0.0f)
        {
            source.volume = NormalizeValueInRange(timer, 0.0f, duration, 0.0f, startingVolume);
            yield return null;
            timer -= Time.deltaTime;
        }
    }

    public static IEnumerator Shake(Transform target, float duration, float intensity, bool horizontal = false, bool vertical = false)
    {
        Vector2 previousShake = Vector2.zero;
        
        float timer = 0.0f;
        while (timer <= duration)
        {
            Vector2 direction = Random.insideUnitCircle;

            if (horizontal)
                direction.y = 0.0f;

            if (vertical)
                direction.x = 0.0f;

            target.position += ((direction * intensity) - previousShake).ToVector3();
            previousShake = direction * intensity;
            
            yield return null;
            timer += Time.deltaTime;
        }

        target.position -= previousShake.ToVector3();
    }

    public static void RestoreTextColor(TextMeshPro text)
    {
        int characterCount = text.textInfo.characterCount;
        TMP_CharacterInfo[] info = text.textInfo.characterInfo;
        
        Color32 white = new Color32(255, 255, 255, 255);
        for (int i = 0; i < characterCount; ++i)
        {
            int meshIndex = info[i].materialReferenceIndex;
            int vertexIndex = info[i].vertexIndex;
   
            Color32[] vertexColors = text.textInfo.meshInfo[meshIndex].colors32;
            vertexColors[vertexIndex + 0] = white;
            vertexColors[vertexIndex + 1] = white;
            vertexColors[vertexIndex + 2] = white;
            vertexColors[vertexIndex + 3] = white;
        }
        
        text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
    
    public static Vector3 LerpVector3(Vector3 start, Vector3 end, float t)
    {
        Vector3 current;
        current.x = Mathf.Lerp(start.x, end.x, t);
        current.y = Mathf.Lerp(start.y, end.y, t);
        current.z = Mathf.Lerp(start.z, end.z, t);

        return current;
    }
    
    public static Vector2 LerpVector2(Vector2 start, Vector2 end, float t)
    {
        Vector2 current;
        current.x = Mathf.Lerp(start.x, end.x, t);
        current.y = Mathf.Lerp(start.y, end.y, t);

        return current;
    }
    
    public static Quaternion ShortestRotation(Quaternion a, Quaternion b)
    {
        if (Quaternion.Dot(a, b) < 0)
        {
            return a * Quaternion.Inverse(Multiply(b, -1));
        }

        else return a * Quaternion.Inverse(b);
    }
    
    public static Vector2 ComputeClosestPointOnLine(Vector2 linePosition, Vector2 lineDirection, Vector2 position)
    {
        Vector2 delta = position - linePosition;
        float dot = Vector2.Dot(delta, lineDirection);
        return linePosition + (lineDirection * dot);
    }
    
    public static Quaternion Multiply(Quaternion input, float scalar)
    {
        return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
    }
}
