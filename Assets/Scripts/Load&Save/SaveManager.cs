using System.IO;
using UnityEngine;


public class SaveManager
{
    /* En esta clase he borrado las constantes para usarlas desde otro script. Lo hice así
     * ya que tenía la necesidad de llamar la constante SAVE_NAME en otro lado
     * creo que es más eficiente así.
     * 
     * Por otro lado he utilizado ? para aprender a darle uso. Lo uso en 109 y 114
     * 
     * He usado el método GetSaveFiles debido a que repetía ese código en ArrayOfSaves y GetLastSave
     * 
     * También utilizado var en muchos casos debido a que es lo que me han recomendado.
     * 
     * He creado una carpeta para imágenes ya que quiero mostrar una captura en la lista de guardados.
     * Para esto primero he tenido que crear _imageFolder y añadir un condicional para comprobar si 
     * dichar carpeta existe. 
     * Tras esto, he creado el método SaveImage() y al copiar su contenido repitía el código del nuevo método
     * GetNumberOfSaves(). 
     * Luego decidí crear una global _saveCount para guardar dicho número cuando creara la imagen. 
     * Esto es así ya que primero entro en SaveImage() al crear el SaveObject en la clase GameManager.Save([...])
     * La intención de hacerlo antes con SaveImage() es que quiero guardar dentro del archivo json la dirección
     * de la imagen generada. Más tarde en GameManager.Save([...]) llamo al método Save y genero el archivo .txt
     * 
     * El string saveImage tenía que ser una ruta concreta ya que utilizaba el objeto TextureImporter. Sucedía debido a que
     * al terminar de convertir a Texture2D y Sprite los png guardados tenía que usar Resources.Load. Esto me obligaba que
     * el archivo estuviera en la carpeta Resources y que en el anterior código SaveImages devolviera un string [] con dos array.
     * En el primero iba la ruta como está ahora y en el segundo solo necesitaba el nombre del archivo sin su tipo. Ej: image_01.
     * Conseguí que funcionara a medias por una razón que me robó mucho tiempo:
     * 1.- Cuando generaba los archivos al guardar, no me mostraba en la lista de botones las imágenes. 
     * La razón era que TextureImporter dependía de un método estático llamado GetAtPath de la libería de Unity. 
     * Esto era un grave problema debido a que mi objeto era null durante MUCHÍSIMO tiempo cuando se generaba la imagen.
     * Y si no era null, era porque iniciaba la aplicación por segunda vez. Ocurría por culpa de lo lento que actualiza los Assets el Unity. 
     * Mi solución fue añadir una condicional que sacaba del método en el que utilizaba TextureImporter:
     *              if(importer == null || importer.TextureType != TextureImporterType.Default){ return; }
     *              
     * Con esto buscaba evitar el error y no entrar en el código que convertía la foto a 2D y Sprite. Pero esto lo que hacía era
     * evitar errores críticos. Al mostrar el botón, la imagen de este estaba vacía. Entonces me di cuenta de una cosa:
     * No tenía el mismo problema con los archivos txt aunque en la ventana Project no se me generara la carpeta ni los archivos. Abrí 
     * el explorador de windows y ví que los archivos estaban ahí, incluida las fotos. La diferencía era que el txt no dependía de que los
     * Assets de la ventana de Project se hubieran actuliazado. Entonces procedí a cambiar la forma en la que recogía los datos de la foto.
     * 
     * Pasé de un método de 6/8 líneas que dependía de:
     *              TextureImporter importer = new TextureImporter.GetAtPath(path);
     * y enviar en SaveImage un array con dos valores a un método de dos líneas que envíaba el path de la imagen guardado en el txt
     * para recoger un array de bytes y con un Texture2D crear un Sprite que enviaba a la imagen del botón.
     * 
     * TODO: Comprobar si puedo cambiar a otro Path de _imageFolder para reducir el código y posiblemente eliminar un método.
     */



    private static readonly string _saveFolder = Application.dataPath + Constants.SaveFiles.SAVE_FINAL_PATH;
    private static readonly string _imageFolder = Application.dataPath + Constants.SaveFiles.IMAGE_FINAL_PATH;

    private static int _saveCount;

    private static int GetNumberOfSaves()
    {
        _saveCount = 0;
        while (File.Exists(_saveFolder + Constants.SaveFiles.SAVE_NAME + _saveCount + Constants.SaveFiles.SAVE_EXTENSION))
            _saveCount++;
        return _saveCount;
    }

    private static FileInfo[] GetSaveFiles()
    {
        var directoryInfo = new DirectoryInfo(_saveFolder);
        var saveFiles = directoryInfo.GetFiles("*" + Constants.SaveFiles.SAVE_EXTENSION);
        return saveFiles;
    }

    public static void Init()
    {
        if (!Directory.Exists(_saveFolder))
        {
            Directory.CreateDirectory(_saveFolder);

        }
        if (!Directory.Exists(_imageFolder))
        {
            Directory.CreateDirectory(_imageFolder);
        }
    }

    public static void Save(string saveString)
    {

        File.WriteAllText(_saveFolder + Constants.SaveFiles.SAVE_NAME + _saveCount + Constants.SaveFiles.SAVE_EXTENSION, saveString);
    }

    public static string SaveImage()
    {
        GetNumberOfSaves();

        string saveImageName = Constants.SaveFiles.ASSETS_PATH_TO_IMAGE_NAME + _saveCount + Constants.SaveFiles.IMAGE_EXTENSION;
        ScreenCapture.CaptureScreenshot(saveImageName);
        return saveImageName;
    }

    public static string GetLastSave()
    {
        var saveFiles = GetSaveFiles();
        FileInfo mostRecentFile = null;

        foreach (var fileInfo in saveFiles)
        {
            mostRecentFile ??= fileInfo;
            if (fileInfo.LastWriteTime > mostRecentFile.LastWriteTime)
                mostRecentFile = fileInfo;
        }

        return mostRecentFile == null ? null : File.ReadAllText(mostRecentFile.FullName);
    }

    public static string[] ArrayOfSaves()
    {
        var saveFiles = GetSaveFiles();
        var arraySaves = new string[saveFiles.Length];

        for (int i = 0; i < saveFiles.Length; i++)
            arraySaves[i] = File.ReadAllText(saveFiles[i].FullName);

        return arraySaves;
    }

    public static byte[] GetImageFromPath(string path)
    {
        var imageInBytes = File.ReadAllBytes(path);
        return imageInBytes;
    }
}