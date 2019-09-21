using System;
using System.Collections.Generic;



namespace csharpcore
{
    public class GildedRose
    {
        IList<Item> Items;
        public GildedRose(IList<Item> Items)
        {
            this.Items = Items;
        }

        //TIP 1: Parralell loop since items does not interact with each other, this way faster loop execution using all threads instead 1
        //TIP 2: Using LINQ split method in 'submethods' and then select each item and do action with it, code will be clearer and more and easier changes to EACH item would be
        //TIP 3: For less assignment it could be created var myItem = Items[i]; after completing all stages with myItem on the end Items[i] = myItem;
        //TIP 4: IF possible DailyUpdateQuality,UpdateSellIn,UpdateQualityByDays add directly to item class so code could be cleaner.
        //TIP 5: For more performance change list to ConcurrentDictionary<int,Item>, but I prefer ObservableCollection<Item>, because no index needed to provide for list, using ConcurrentDictionary uses multiple if possible threads.

        public void UpdateQuality()
        {
            for (var i = 0; i < Items.Count; i++)
            {

                switch (Items[i].Name)
                {
                    case "Sulfuras, Hand of Ragnaros":
                    {
                        break;
                    }
                    case "Aged Brie":
                    {
                        Items[i] = DailyUpdateQuality(Items[i]);
                        Items[i] = UpdateSellIn(Items[i]);
                        Items[i] = UpdateQualityByDays(Items[i],0);
                        break;
                    }
                    case "Backstage passes to a TAFKAL80ETC concert":
                    {
                        Items[i] = DailyUpdateQuality(Items[i]);
                        Items[i] = UpdateQualityByDays(Items[i], 11);
                        Items[i] = UpdateQualityByDays(Items[i], 6);
                        
                        Items[i] = UpdateSellIn(Items[i]);

                        int quality = Items[i].Quality;
                        int sellin = Items[i].SellIn;

                        Items[i].Quality = sellin < 0 ? 0 : quality;
                        break;
                    }
                    case "Conjured Mana Cake":
                    {
                        Items[i] = DailyUpdateQuality(Items[i], 0, false,2);

                        Items[i] = UpdateSellIn(Items[i]);

                        Items[i] = UpdateQualityByDays(Items[i], 0, 0, false,2);
                        break;
                    }
                    case var _ when Items[i].Name != "Aged Brie":
                    case var _ when Items[i].Name != "Backstage passes to a TAFKAL80ETC concert":
                    {
                        Items[i] = DailyUpdateQuality(Items[i], 0, false);
                        
                        Items[i] = UpdateSellIn(Items[i]);

                        Items[i] = UpdateQualityByDays(Items[i], 0, 0, false);
                        break;
                    }

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentitem">Current Item from List</param>
        /// <returns>Updated item with new SellIn value</returns>
        private Item UpdateSellIn(Item currentitem)
        {
            if(currentitem.Name != "Sulfuras, Hand of Ragnaros")
            {
                currentitem.SellIn--;
            }
            return currentitem;
        }

        /// <summary>
        /// Main method for updating quality in first stage when sellin param matters
        /// </summary>
        /// <param name="currentitem">Current Item from List</param>
        /// <param name="RemainingDay">Update quality if SellIn is more or less, depends on DoBelowQualityAction selection</param>
        /// <param name="UpdateQuality">Update quality if UpdateQuality is more or less, depends on DoBelowQualityAction selection</param>
        /// <param name="DoBelowQualityAction">If true all checks done BELOW params above, if false all checks done ABOVE</param>
        /// <param name="QualityChangingSpeed">Quality degradation speed, normal items 1</param>
        /// <returns></returns>
        private Item UpdateQualityByDays(Item currentitem,int RemainingDay, int UpdateQuality = 50, bool DoBelowQualityAction = true,int QualityChangingSpeed = 1)
        {
            if (DoBelowQualityAction)
            {
                if ((currentitem.SellIn < RemainingDay) && (currentitem.Quality < UpdateQuality))
                {
                    currentitem.Quality+= QualityChangingSpeed;
                }
            }
            else
            {
                if ((currentitem.SellIn < RemainingDay) && (currentitem.Quality > UpdateQuality))
                {
                    currentitem.Quality -= QualityChangingSpeed;
                }
            }
            return currentitem;
        }

        /// <summary>
        /// Second stage for Quality update, sellin param does not matter
        /// </summary>
        /// <param name="currentitem">Current Item from List</param>
        /// <param name="UpdateQuality">Update quality if UpdateQuality is more or less, depends on DoBelowQualityAction selection</param>
        /// <param name="DoBelowQualityAction">If true all checks done BELOW params above, if false all checks done ABOVE</param>
        /// <param name="QualityChangingSpeed">Quality degradation speed, normal items 1</param>
        /// <returns></returns>
        private Item DailyUpdateQuality(Item currentitem, int UpdateQuality = 50,bool DoBelowQualityAction = true, int QualityChangingSpeed = 1)
        {
            if (DoBelowQualityAction)
            {
                if (currentitem.Quality < UpdateQuality)
                {
                    currentitem.Quality += QualityChangingSpeed;
                }
            }
            else
            {
                if (currentitem.Quality > UpdateQuality)
                {
                    currentitem.Quality -= QualityChangingSpeed;
                }
            }
            return currentitem;
        }
    }
}
