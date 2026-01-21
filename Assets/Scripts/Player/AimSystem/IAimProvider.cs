using UnityEngine;

public interface IAimProvider
{
    Transform AimOrigin { get; }   //звідки стріляємо
    //float MaxAimDistance { get; }  //поки не вирішив чи хочу його тут

    void OnAimUpdated(AimResult result);
}