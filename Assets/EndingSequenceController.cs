using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EndingSequenceController : MonoBehaviour
{
    [Header("👻 鬼出现动画")]
    public GameObject ghostObject;

    [Header("😱 贴脸杀图像")]
    public CanvasGroup screamImage;

    [Header("📷 温馨家庭照片")]
    public CanvasGroup familyPhoto;

    [Header("🏠 房子（需隐藏）")]
    public GameObject[] houseObjectsToHide;

    [Header("🕳 黑屏 UI")]
    public CanvasGroup blackScreen;

    [Header("⌨ 打字文字")]
    public TMP_Text typewriterText;
    [TextArea]
    public string messageToType;
    public float typingSpeed = 0.05f;

    [Header("📞 热线信息")]
    public GameObject hotlineInfo;

    [Header("🎵 第二段音乐（可选）")]
    public AudioSource secondMusic;

    void Start()
    {
        // 初始设置
        if (ghostObject != null) ghostObject.SetActive(false);
        if (screamImage != null) { screamImage.alpha = 0; screamImage.gameObject.SetActive(false); }
        if (familyPhoto != null) familyPhoto.alpha = 0;
        if (blackScreen != null) blackScreen.alpha = 0;
        if (typewriterText != null) typewriterText.text = "";
        if (hotlineInfo != null) hotlineInfo.SetActive(false);
        if (secondMusic != null) secondMusic.Stop();

        StartCoroutine(PlayEndingSequence());
    }

    IEnumerator PlayEndingSequence()
    {
        // Step 1：鬼探头
        ghostObject.SetActive(true);
        yield return new WaitForSeconds(2f);

        // Step 2：贴脸杀图像（时间加长）
        screamImage.gameObject.SetActive(true);
        yield return StartCoroutine(FadeCanvasGroup(screamImage, 0f, 1f, 0.4f));
        yield return new WaitForSeconds(1.6f); // 原来是0.6s
        yield return StartCoroutine(FadeCanvasGroup(screamImage, 1f, 0f, 0.5f));
        screamImage.gameObject.SetActive(false);

        // Step 3：家庭照片（半透明出现）
        yield return StartCoroutine(FadeCanvasGroup(familyPhoto, 0f, 0.4f, 1f));
        yield return new WaitForSeconds(5f);

        // Step 4：隐藏房子
        foreach (GameObject obj in houseObjectsToHide)
        {
            if (obj != null) obj.SetActive(false);
        }

        // Step 5：黑屏盖上
        yield return StartCoroutine(FadeCanvasGroup(blackScreen, 0f, 1f, 1f));

        // Step 6：播放音乐
        if (secondMusic != null) secondMusic.Play();

        // Step 7：打字机
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(TypeText());

        // Step 8：热线信息
        hotlineInfo.SetActive(true);
    }

    IEnumerator FadeCanvasGroup(CanvasGroup group, float from, float to, float duration)
    {
        float elapsed = 0f;
        group.alpha = from;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            group.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        group.alpha = to;
    }

    IEnumerator TypeText()
    {
        typewriterText.text = "";
        foreach (char c in messageToType)
        {
            typewriterText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}