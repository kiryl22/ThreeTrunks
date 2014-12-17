using ThreeTrunks.Data.Enums;
using ThreeTrunks.Data.Models;

namespace ThreeTrunks.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ThreeTrunks.Data.ThreeTrunksContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ThreeTrunksContext context)
        {
            context.Settings.AddOrUpdate(s => s.Id, new Settings()
            {
                Id = 1,
                SettingKey = "IsPublic",
                SettingValue = (false).ToString(),
                SettingCaption = "���� �������� �����������",
                SettingType = SettingTypes.Boolean
            });

            context.Settings.AddOrUpdate(s => s.Id, new Settings()
            {
                Id = 2,
                SettingKey = "UnavailableMessage",
                SettingCaption = "��������� ��� �����������",
                SettingType = SettingTypes.Text
            });

            context.Settings.AddOrUpdate(s => s.Id, new Settings()
            {
                Id = 3,
                SettingKey = "ContactPhone",
                SettingCaption = "���������� �������",
                SettingType = SettingTypes.Text
            });

            context.ImageCategories.AddOrUpdate(e => e.Id, new ImageCategory()
            {
                Id = 1,
                Name = "��������",
                IsGallery = false
            });

            context.ImageCategories.AddOrUpdate(e => e.Id, new ImageCategory()
            {
                Id = 2,
                Name = "�������",
                IsGallery = true

            });

            context.ImageCategories.AddOrUpdate(e => e.Id, new ImageCategory()
            {
                Id = 3,
                Name = "�������",
                IsBasket = true

            });

            context.SaveChanges();
        }
    }
}
