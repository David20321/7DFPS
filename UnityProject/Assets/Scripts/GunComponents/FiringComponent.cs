using UnityEngine;

[GunData(GunAspect.FIRING)]
public class FiringComponent : GunComponent {
    public AudioClip[] sound_gunshot_bigroom = new AudioClip[0];
    public AudioClip[] sound_gunshot_smallroom = new AudioClip[0];
    public AudioClip[] sound_gunshot_open = new AudioClip[0];

    [IsNonNull, HasTransformPath("point_muzzleflash")] public Transform point_muzzleflash;
    [IsNonNull, HasTransformPath("point_muzzle")] public Transform point_muzzle;

    [IsNonNull] public GameObject bullet_obj;
    [IsNonNull] public GameObject muzzle_flash;

    public float exit_velocity = 251f;
    public int projectile_count = 1;
    public float inaccuracy = 0f;

    [Range(-2, 2)] public float recoil_multiplier = 1f;

    //Incremented everytime a round is successfully launched.
    internal int fire_count = 0;
}