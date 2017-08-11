using System;
using System.Web;
using VkNet;
using VkNet.Enums.SafetyEnums;

namespace SuicideAlpha
{
    class Wall
    {
        VkApi app;
        VkNet.Model.WallGetObject wall;

        public string GetTextWall()
        {
            string result = String.Empty;

            foreach (var posts in wall.WallPosts)
            {
                if (posts.Text != "")
                    result += posts.Text + "\n\n";

                foreach (var p in posts.CopyHistory)
                    if (p.Text != "")
                        result += p.Text + "\n\n";
            }

            return result;

        }

        public HtmlString GetText()
        {
            return new HtmlString("sda");
        }

        public Wall(VkApi app, long? ID)
        {
            this.app = app;
            wall = app.Wall.Get(new VkNet.Model.RequestParams.WallGetParams
            {
                OwnerId = ID,
                Count = 5,
                Extended = true,
                Filter = WallFilter.Owner
            });
        }

        public Wall(VkApi app, string domain)
        {
            this.app = app;
            wall = app.Wall.Get(new VkNet.Model.RequestParams.WallGetParams
            {
                Domain = domain,
                Count = 99,
                Extended = true,
                Filter = WallFilter.Owner
            });
        }
    }
}
