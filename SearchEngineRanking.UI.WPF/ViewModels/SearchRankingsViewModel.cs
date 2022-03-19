using System.ComponentModel;

namespace SearchEngineRanking.WPF.ViewModels
{
  public class SearchRankingsViewModel : INotifyPropertyChanged, IDataErrorInfo
  {
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public SearchRankingsViewModel()
    {
      _companyURL = "";
      _keyWords = "";
    }

    private string _companyURL;

    public string CompanyURL
    {
      get { return _companyURL; }
      set
      {
        if (value != _companyURL)
        {
          _companyURL = value;
          OnPropertyChanged("CompanyURL");
        }
      }
    }

    private string _keyWords;

    public string KeyWords
    {
      get { return _keyWords; }
      set
      {
        if (value != _keyWords)
        {
          _keyWords = value;
          OnPropertyChanged("KeyWords");
        }
      }
    }

    string IDataErrorInfo.Error
    {
      get { return null!; }
    }

    string IDataErrorInfo.this[string columnName]
    {
      get
      {
        if (columnName == "CompanyURL")
        {
          // Validate property and return a string if there is an error
          if (string.IsNullOrEmpty(CompanyURL))
          {
            return "Company URL is required";
          }
        }

        if (columnName == "KeyWords")
        {
          // Validate property and return a string if there is an error
          if (string.IsNullOrEmpty(KeyWords))
          {
            return "KeyWords are required";
          }
        }

        // If there's no error, null gets returned
        return null!;
      }
    }
  }
}
