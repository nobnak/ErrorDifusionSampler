using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ErrorDiffusionSamplerSys {

    public class Validator {

        public event System.Action Validation;
        public event System.Action Validated;

        protected bool validated;

        public Validator(bool validated = false) {
            this.validated = validated;
        }

        public void Invalidate() => validated = false;
        public void Validate() {
            if (validated) return;
            validated = true;

            Validation?.Invoke();

            if (validated) Validated?.Invoke();
        }
        public void Reset() {
            Validation = null;
            Validated = null;
        }
    }

}
