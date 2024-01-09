using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite coinImg;
    public Sprite hookImg;
    public Sprite mapImg;
    public Sprite boneImg;
    public Sprite mushroomImg;
    public Sprite totemImg;
    public Sprite soulImg;
    public Sprite gemImg;
    public Sprite waterImg;

    public GameObject menu;
    public TMP_Text title;
    public TMP_Text desc;
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClosePressed(){
        menu.SetActive(false);
    }
    public void showItemMenu(string item){
        menu.SetActive(true);
        title.text = item;
        if(item.Equals("First Gold Coin")){
            desc.text = "Ah the Captains first dabloon! He dug it up from far south with his crew back in '65. The dabloon's been in his pocket since the day he found it, how on earth did it fall out? The Captain believes that it's very lucky, maybe a few more dabloons will come flying your way!";
        }else if(item.Equals("Old Hook Hand")){
            desc.text = "The Captains old hook! It might be a bit worn and crooked but its as good as new in his eyes. Don't go swaying it around because it'll definitely poke a few eyes out! ";
        }else if(item.Equals("Treasure Map")){
            desc.text = "A Treasure Map! Looking at the wear and tear of this map suggests that the treasure is probably gone already. But who knows? Doesn't hurt to do a bit of exploring.";
        }else if(item.Equals("Bone Pile")){
            desc.text = "BONES! The King loves some old bones. They sure do make a funny noise when hit together, right? Plus its a symbol of how many enemies he's defeated. Oh... these are quite a few bones... I'd be careful around him.";
        }else if(item.Equals("Totem")){
            desc.text = "A totem. A very sacred object belonging to the goblins. Be very careful around this and return it back to the Goblin King as soon as possible.";
        }else if(item.Equals("Weird Mushroom")){
            desc.text = "A purple mushroom? What a rarity! The most delicious, scrumpious mushroom in all the lands, only for goblins though. (Please do not consume purple mushrooms).";
        }else if(item.Equals("Souls in a Bottle")){
            desc.text = "Souls in a bottle. I wonder how many are in there? Don't go opening this around unless you want to be haunted for the rest of your life.";
        }else if(item.Equals("Dark Gem")){
            desc.text = "Oh wow a dark gem! This is definintely worth an arm or two. Becareful where you keep it, it attracts ghosts and demons.";
        }else if(item.Equals("Unholy Water")){
            desc.text = "Unholy Water. It's just like water, but very very very evil... You probably should just give it back to the demon, unless you want to become one yourself.";
        }
    }
}
