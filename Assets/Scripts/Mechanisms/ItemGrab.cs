using System;
using Gravity;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Class making an object grabbable by the player
/// </summary>
[RequireComponent(typeof(CustomGravityRigidbody))]
public class ItemGrab : MonoBehaviour
{
    [SerializeField] private float maxSpeedAdjustment = 10f, maxAccelerationAdjustment = 10f;

    //todo change that call from a string to an int or a list ;__;
    [SerializeField] private string nameLayerNoCollisionsPlayer = "Player";
    [SerializeField] private float percentTransparent = 0.5f;
    [SerializeField] private AudioClip[] grabSounds;
    [SerializeField] private AudioClip[] collisionSounds;
    protected Rigidbody Body;
    private MeshRenderer[] _renderers;
    private bool _isGrabbed;
    protected CustomGravityRigidbody CustomGravity;
    private float _dist;
    private Transform _grabAnchor;
    private LayerMask _originLayerMask;
    private AudioSource _audioSource;

    private void Awake()
    {
        Body = GetComponent<Rigidbody>();
        _renderers = GetComponents<MeshRenderer>();
        CustomGravity = GetComponent<CustomGravityRigidbody>();
        _originLayerMask = gameObject.layer;
        _audioSource = GetComponentInChildren<AudioSource>();
    }

    /// <summary>
    /// Grabs the object with a reference transform as an anchor
    /// </summary>
    /// <param name="anchor">the anchor</param>
    public virtual void Grab(Transform anchor)
    {
        PlayOneSoundRandom(grabSounds);
        _grabAnchor = anchor;
        _dist = Vector3.Distance(anchor.position, transform.position);
        _isGrabbed = true;
        SwitchState();
        Body.velocity = Vector3.zero;
    }

    /// <summary>
    /// Drops the object while adding some velocity to it
    /// </summary>
    /// <param name="velocity">the velocity to add</param>
    public void Drop(Vector3 velocity)
    {
        _isGrabbed = false;
        SwitchState();
        Body.velocity += velocity;
    }

    /// <summary>
    /// Throws the object with a force being added
    /// </summary>
    /// <param name="strength">the strength applied to the object</param>
    public virtual void Throw(float strength)
    {
        Body.AddForce((transform.position - _grabAnchor.position).normalized * strength);
    }

    private void FixedUpdate()
    {
        MoveUpdate();
    }

    protected void MoveUpdate()
    {
        if (!_isGrabbed)
        {
            return;
        }

        Vector3 aim = _grabAnchor.forward * _dist - (transform.position - _grabAnchor.position);

        float maxSpeedChange = maxAccelerationAdjustment * Time.fixedDeltaTime;
        Body.velocity = Vector3.MoveTowards(Body.velocity, aim * maxSpeedAdjustment, maxSpeedChange);
    }

    private void SwitchState()
    {
        ChangeLayers(transform, _isGrabbed
            ? nameLayerNoCollisionsPlayer
            : LayerMask.LayerToName(_originLayerMask));
        /*
        gameObject.layer = LayerMask.NameToLayer(_isGrabbed
            ? nameLayerNoCollisionsPlayer
            : LayerMask.LayerToName(_originLayerMask));
        */
        Body.interpolation = _isGrabbed ? RigidbodyInterpolation.Interpolate : RigidbodyInterpolation.None;
        CustomGravity.EnableGravity = !_isGrabbed;
        foreach (var r in _renderers)
        {
            Color c = r.material.color;
            r.material.color = new Color(c.r, c.g, c.b, _isGrabbed ? percentTransparent : 1f);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        PlayOneSoundRandom(collisionSounds);
    }

    private void PlayOneSoundRandom(AudioClip[] sounds)
    {
        _audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
    }

    private void ChangeLayers(Transform t, string nameLayer)
    {
            t.gameObject.layer = LayerMask.NameToLayer(nameLayer);
            for (int i = 0; i < t.childCount; i++)
            {
                ChangeLayers(t.GetChild(i), nameLayer);
            }
    }
}