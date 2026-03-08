namespace ServerApp.Models
{
    public class Config
    {
        public static readonly string[] AllowedOrigins =
        [
            "http://localhost:3000",    // Node.js, Next.js, Nuxt.js, Express.js... default
            "http://localhost:5173",    // Vite default
            "http://localhost:5174"     // VueApp
        ];
    }
}
