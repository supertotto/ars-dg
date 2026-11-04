// claude: arquivo criado pelo Claude (app claude.ai)
using System.IO;
using UnityEditor;

namespace ARS.EditorTools
{
    /// <summary>Pastas padrão do projeto. Mudou aqui, muda em todas as ferramentas.</summary>
    public static class ArsPaths
    {
        public const string Personagens = "Assets/Characters";
        public const string Animacoes = "Assets/Animations";
        public const string AnimacoesComuns = "Assets/Animations/Common";
        public const string Prefabs = "Assets/Prefabs/Heroes";
        public const string Cenas = "Assets/Scenes";
        public const string Dados = "Assets/Data";
        public const string Ui = "Assets/UI"; // claude: imagens da interface (entram como Sprite)
        public const string HudPrefab = "Assets/Prefabs/UI/HUD.prefab"; // claude

        // claude: prefabs do pack Vampire usados como Lorde Vampiro (uma por skin)
        public static readonly string[] SkinsDoLordeVampiro =
        {
            "Assets/Vampire/Prefab/Vampyre Skin1 Prefab.prefab",
            "Assets/Vampire/Prefab/Vampyre Skin2 Prefab.prefab",
            "Assets/Vampire/Prefab/Vampyre Skin3 Prefab.prefab",
        };

        public static readonly string[] SubpastasDoHeroi = { "Model", "Textures", "Animations", "Materials" };

        /// <summary>Cria a pasta (e as pastas-pai) dentro de Assets, se não existir.</summary>
        public static void GarantirPasta(string caminho)
        {
            caminho = caminho.Replace('\\', '/').TrimEnd('/');
            if (AssetDatabase.IsValidFolder(caminho)) return;
            string pai = Path.GetDirectoryName(caminho)?.Replace('\\', '/');
            string nome = Path.GetFileName(caminho);
            if (!string.IsNullOrEmpty(pai)) GarantirPasta(pai);
            AssetDatabase.CreateFolder(pai, nome);
        }

        /// <summary>Ex.: "Assets/Characters/Caleb/Model/caleb.fbx" → "Caleb".</summary>
        public static string HeroiDoCaminho(string caminho)
        {
            caminho = caminho.Replace('\\', '/');
            string prefixo = Personagens + "/";
            if (!caminho.StartsWith(prefixo)) return null;
            string resto = caminho.Substring(prefixo.Length);
            int barra = resto.IndexOf('/');
            return barra > 0 ? resto.Substring(0, barra) : null;
        }

        public static bool EhModeloDeHeroi(string caminho) =>
            HeroiDoCaminho(caminho) != null && caminho.Replace('\\', '/').Contains("/Model/");

        public static bool EhAnimacao(string caminho)
        {
            caminho = caminho.Replace('\\', '/');
            return caminho.StartsWith(Animacoes + "/") ||
                   (HeroiDoCaminho(caminho) != null && caminho.Contains("/Animations/"));
        }
    }
}
