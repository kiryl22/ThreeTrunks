using System;

namespace ThreeTrunks.Data.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private readonly ThreeTrunksContext context = new ThreeTrunksContext();
        private readonly UserRepository userRepository;
        private readonly ImageRepository imageRepository;
        private readonly ImageCategoryRepository imageCategoryRepository;
        private readonly SettingsRepository settingsRepository;

        public UnitOfWork()
        {
            this.userRepository = new UserRepository(this.context);
            this.imageRepository = new ImageRepository(this.context);
            this.imageCategoryRepository = new ImageCategoryRepository(this.context);
            this.settingsRepository = new SettingsRepository(this.context);
        }

        public UserRepository UserRepository
        {
            get { return this.userRepository; }
        }

        public ImageRepository ImageRepository
        {
            get { return this.imageRepository; }
        }

        public ImageCategoryRepository ImageCategoryRepository
        {
            get { return this.imageCategoryRepository; }
        }

        public SettingsRepository SettingsRepository
        {
            get { return this.settingsRepository; }
        }

        public int Save()
        {
            return this.context.SaveChanges();
        }

        public ThreeTrunksContext Context
        {
            get
            {
                return this.context;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
