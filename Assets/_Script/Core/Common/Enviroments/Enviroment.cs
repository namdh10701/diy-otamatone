namespace Core.Env
{
    public static class Environment
    {
        public enum Env
        {
            DEV, TEST, PROD
        }
        private static Env _env;
        public static Env ENV=> _env;
        public static void SetEnvironment(Env env)
        {
            _env = env;
        }
    }
}