using UnityEngine;
using System.Collections;
using System.Collections.Specialized;

public class CatHead_Transition : Transition {

  protected override IEnumerator TransitionOut() {
    while (true) {
      transform.localScale += new Vector3(0.025f, 0.025f, 0);
      transform.Rotate(Vector3.forward * 4.4f);

      if (transform.localScale.x > 2f) {
        TransitionOutComplete();
        break;
      }
      else
        yield return null;
    }
  }

  protected override IEnumerator TransitionIn() {
    while (true) {
      transform.localScale -= new Vector3(0.025f, 0.025f, 0);
      transform.Rotate(Vector3.forward * 4.4f);

      if (transform.localScale.x <= 0.005f) {
        TransitionInComplete();
        break;
      }
      else
        yield return null;
    }
  }

}
