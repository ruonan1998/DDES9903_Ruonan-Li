using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EndingManager : MonoBehaviour
{
    [Header("鬼脸相关")]
    public GameObject ghostSmile;             // 鬼从床边探出的图（GameObject）
    public CanvasGroup screamImage;           // 大叫贴脸杀图（UI Canvas Image）

    [Header("房间整体")]
    public GameObject houseGroup;             // 所有房间模型的父物体

    [Header("黑屏 + 音乐")]
    public CanvasGroup blackScreen;           // 黑色全屏 UI CanvasGroup
    public AudioSource secondMusic;           // 阴郁音乐

    [Header("打字文字")]
    public TextMeshProUGUI typewriterText;    // 打字显示用的 TextMeshPro 文本
    [TextArea(4, 10)]
    public string[] hotlineTexts;             // 多行热线信息

    [Header("结尾照片")]
    public CanvasGroup familyPhoto;           // 最后温馨照片的 UI Image

    void Start()
    {
        StartCoroutine(RunEndingSequence());
    }

    IEnumerator RunEndingSequence()
    {
        yield return new WaitForSeconds(2f);           // 初始延迟
        ghostSmile.SetActive(true);                    // 鬼探头

        yield return new WaitForSeconds(1.5f);         // 延迟再贴脸杀
        screamImage.gameObject.SetActive(true);

        houseGroup.SetActive(false);                   // 房间隐藏

        yield return new WaitForSeconds(1f);
        yield return FadeCanvas(blackScreen, 1f);      // 黑屏淡入
        secondMusic.Play();                            // 第二段音乐播放

        yield return new WaitForSeconds(1f);
        yield return TypeText();                       // 打字机显示热线

        yield return new WaitForSeconds(10f);
        familyPhoto.gameObject.SetActive(true);        // 温馨照片淡入
        yield return FadeCanvas(familyPhoto, 2f);
    }

    IEnumerator FadeCanvas(CanvasGroup group, float duration)
    {
        float t = 0;
        group.alpha = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(0, 1, t / duration);
            yield return null;
        }
        group.alpha = 1;
    }

    IEnumerator TypeText()
    {
        foreach (string line in hotlineTexts)
        {
            typewriterText.text = "";
            foreach (char c in line)
            {
                typewriterText.text += c;
                yield return new WaitForSeconds(0.04f); // 每个字间隔
            }
            yield return new WaitForSeconds(1.5f); // 每行间隔
        }
    }
}