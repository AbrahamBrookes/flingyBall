using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace flingyball
{
    public class GameUIItem : MonoBehaviour
    {

        public Animation anims;

        // Start is called before the first frame update
        void Start()
        {
            anims = GetComponent<Animation>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
