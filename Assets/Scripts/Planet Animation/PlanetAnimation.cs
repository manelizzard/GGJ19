using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlanetAnimation : MonoBehaviour
{
    public float planetDrainAnimationDuration = 60;

    public Transform innerPlanetTransform;
    public Transform initialModels;
    public Transform middleModels;
    public Transform endModels;
    public bool draining = false;
    public MeshRenderer[] planetMeshRenderers;
    
    public float currentPlanetEnergy = 100;
    public float maxPlanetEnergy = 100;
    
    private Sequence saturationSeq;
    void Start()
    {

        middleModels.localScale = middleModels.localScale / 2;
        endModels.localScale = endModels.localScale / 2;

        DOTween.Sequence()
            .Append(innerPlanetTransform.DORotate(new Vector3(0, 360, 0),
                20f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear))
            .SetLoops(-1)
            .SetAutoKill(false)
            .Play();

        DOTween.Sequence()
            .AppendInterval(Random.Range(0, 2))
            .Append(DOTween.To(() => transform.localPosition, 
                x => transform.localPosition = x, 
                new Vector3(transform.localPosition.x, transform.localPosition.y + .1f, transform.localPosition.z),
                3f))
            .Append(DOTween.To(() => transform.localPosition, 
                x => transform.localPosition = x, 
                new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z),
                3f))
            .SetEase(Ease.Linear)
            .SetLoops(-1)
            .SetAutoKill(false)
            .Play();
    }

    void Update() 
    {
        if (draining) {
            float diff = maxPlanetEnergy / 2;
            float saturation = Mathf.Lerp(0, 1, (diff - currentPlanetEnergy) / diff);

            float h, s, v;
            Color.RGBToHSV(planetMeshRenderers[0].material.color, out h, out s, out v);
            foreach(MeshRenderer r in planetMeshRenderers) {
                r.material.color = Color.HSVToRGB(h, saturation, v);
            }
        }
    }

    public void StartDrainingAnimation() 
    {
        if (draining == false) {
            DOTween.Sequence()
                .AppendCallback(() => {
                    if (currentPlanetEnergy >= 0) {
                        currentPlanetEnergy -= 1;
                    }
                })
                // Wait
                .AppendInterval(planetDrainAnimationDuration / maxPlanetEnergy)
                .SetLoops(-1)
                .Play();

            DOTween.Sequence()
                .AppendCallback(() => {
                    middleModels.localScale = middleModels.localScale / 2;
                    endModels.localScale = endModels.localScale / 2;
                })
                .Insert(0f, initialModels.DOScale(Vector3.zero, planetDrainAnimationDuration * 3))
                .Insert(planetDrainAnimationDuration / 6f, middleModels.DOScale(middleModels.localScale, planetDrainAnimationDuration / 2))
                .Insert(planetDrainAnimationDuration / 3f, endModels.DOScale(endModels.localScale, planetDrainAnimationDuration / 2))
                .Play();

            draining = true;
        }
    }
}
