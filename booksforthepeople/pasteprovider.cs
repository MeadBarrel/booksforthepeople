new ScriptObject(PasteProvider)
{
};


function PasteProvider::init(%this)
{
  createPath(%this.getBookStoragePath());
}


function PasteProvider::readBook(%this, %tag)
{
  %this.web_last = openWeb(%this.getBookUrl(%tag));
}


function PasteProvider::getBookUrl(%this, %tag)
{
  %book_id = %this.getBookId(%tag);
  return %this.base_url @ %book_id;
}


function PasteProvider::getRawBookUrl(%this, %tag)
{
  %book_id = %this.getBookId(%tag);
  return %this.raw_url @ %book_id;
}



function PasteProvider::getBookId(%this, %tag)
{
  %prefix_len = strlen(%this.prefix);
  return getSubStr(%tag, %prefix_len);
}


function PasteProvider::isCorrectTag(%this, %tag)
{
  return startsWith(%tag, %this.prefix);
}


function PasteProvider::download(%this, %tag)
{
  %downloader = new ScriptObject() {
    class = PasteDownloader;
    tag = %tag;
    provider = %this;
  };
  %downloader.download();
}


function PasteProvider::getBookStoragePath(%this)
{
  return $book_mod_global.mod_folder @ %this.prefix @ "/";
}


function PasteProvider::getBookFilename(%this, %tag)
{
  return %this.getBookStoragePath() @ %tag;
}


function PasteProvider::createPaste(%this)
{
    openWeb(%this.create_paste_url);
}


function PasteDownloader::download(%this, %tag)
{
  echo("DOWNLOADING", %tag);
  %download_helper = new SimDownloadHelper()
  {
    url = %this.provider.getRawBookUrl(%this.tag);
    fileName = %this.provider.getBookFilename(%this.tag);
  };
  echo("URL:", %download_helper.url);
  echo("FILENAME", %download_helper.fileName);
  %download_helper.start();
}
