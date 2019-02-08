using System.Collections;
using System.IO;
using UnityEngine;

public class PictureTaker : MonoBehaviour {

    public Transform target;
    public int distanceMin = 10;
    public int distanceMax = 200;
    public int distanceStep = 10;

    public int angleMin = 0;
    public int angleMax = 70;
    public int angleStep = 10;

    private void Start() {
        StartCoroutine(MoveToTransforms());
    }

    private IEnumerator MoveToTransforms() {
        DirectoryInfo di = new DirectoryInfo("output");
        di.Create();
        foreach (FileInfo file in di.GetFiles()) {
            file.Delete();
        }

        for (int a = angleMin; a <= angleMax; a += angleStep) {
            float rad = Mathf.Deg2Rad * a;
            for (int r = distanceMin; r <= distanceMax; r += distanceStep) {
                string file = string.Format("output/{0:D2}_{1:D4}.png", a, r);
                Debug.Log("Capturing " + file);
                Vector3 offset = new Vector3(r * Mathf.Cos(rad), 0, r * Mathf.Sin(rad));
                transform.position = offset;
                transform.LookAt(target);
                yield return new WaitForEndOfFrame();
                ScreenCapture.CaptureScreenshot(file);
            }
        }
        File.WriteAllText("output/README.txt", $"Image name format: angle_distance.png\n" +
            $"Vertical FOV: {Camera.current.fieldOfView} degrees\n");
    }
}
