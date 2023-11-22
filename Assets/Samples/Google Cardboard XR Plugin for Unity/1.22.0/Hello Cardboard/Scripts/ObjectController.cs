//-----------------------------------------------------------------------
// <copyright file="ObjectController.cs" company="Google LLC">
// Copyright 2020 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections;
using UnityEngine;

/// <summary>
/// Controls target objects behaviour.
/// </summary>
public class ObjectController : MonoBehaviour
{
    /// <summary>
    /// The material to use when this object is inactive (not being gazed at).
    /// </summary>
    public Material InactiveMaterial;

    /// <summary>
    /// The material to use when this object is active (gazed at).
    /// </summary>
    public Material GazedAtMaterial;

    // The objects are about 1 meter in radius, so the min/max target distance are
    // set so that the objects are always within the room (which is about 5 meters
    // across).
    private const float _minObjectDistance = 2.5f;
    private const float _maxObjectDistance = 3.5f;
    private const float _minObjectHeight = 0.5f;
    private const float _maxObjectHeight = 3.5f;


    private bool isGazedAt;
    public float gazeTimer;
    public float gazeDuration = 1.0f; // 5 seconds
    public bool _selected = false;


    private Renderer _myRenderer;
    private Vector3 _startingPosition;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    public void Start()
    {
        _startingPosition = transform.parent.localPosition;
        _myRenderer = GetComponent<Renderer>();
        SetMaterial(false);
    }

    /// <summary>
    /// Teleports this instance randomly when triggered by a pointer click.
    /// </summary>
    public void TeleportRandomly()
    {

            int numSibs = transform.parent.childCount;

            // Calculate the next sibling index
            int nextSibIdx = (transform.GetSiblingIndex() + 1) % numSibs;

            // Get the next sibling
            GameObject nextSib = transform.parent.GetChild(nextSibIdx).gameObject;

            // Moves the parent to the new position (siblings relative distance from their parent is 0).
            // transform.parent.localPosition = nextSib.transform.localPosition;

            nextSib.SetActive(true);
            gameObject.SetActive(false);
            SetMaterial(false);

    }

    /// <summary>
    /// This method is called by the Main Camera when it starts gazing at this GameObject.
    /// </summary>
    public void OnPointerEnter()
    {
        isGazedAt = true;
        SetMaterial(true);
    }

    /// <summary>
    /// This method is called by the Main Camera when it stops gazing at this GameObject.
    /// </summary>
    public void OnPointerExit()
    {
        isGazedAt = false;
        SetMaterial(false);
        gazeTimer = 0f;
    }

    /// <summary>
    /// This method is called by the Main Camera when it is gazing at this GameObject and the screen
    /// is touched.
    /// </summary>
    public void OnPointerClick()
    {
        TeleportRandomly();
    }

    ///
    /// <param name="gazedAt">

    private void SetMaterial(bool gazedAt)
    {
        if (InactiveMaterial != null && GazedAtMaterial != null)
        {
            _myRenderer.material = gazedAt ? GazedAtMaterial : InactiveMaterial;
        }
    }





    private void Update()
    {
        // Check if the object is being gazed at
        if (isGazedAt)
        {
            // Increment the gaze timer
            gazeTimer += Time.deltaTime;

            // Check if the gaze duration has been reached
            if (gazeTimer >= gazeDuration)
            {
                // Perform actions when gazed at for 5 seconds
                OnGazeDurationReached();
            }
        }
        else
        {

            gazeTimer = 0f;
        }

        

    }

    void OnGazeDurationReached()
    {

        Debug.Log("Object gazed at for 5 seconds!");
        // _selected = true;
        TeleportRandomly();
        gazeTimer = 0.0f;

    }

}
