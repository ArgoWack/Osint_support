using Osint_WPF.Model;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Osint_WPF.BeginDataSearch;

namespace Osint_WPF.ViewModel
{
    public class DehashedViewModel
    {
        public readonly Dehashed dehashedService;

        public ObservableCollection<Dehashed.Entry> Entries { get; private set; } = new ObservableCollection<Dehashed.Entry>();
        public ObservableCollection<string> FormattedEntries { get; private set; } = new ObservableCollection<string>();
        public ICommand SearchCommand { get; private set; }

        public DehashedViewModel(string dehashedApiUsername, string dehashedApiKey, string myApiName)
        {
            dehashedService = new Dehashed(dehashedApiUsername, dehashedApiKey, myApiName);
            SearchCommand = new AsyncCommand(async (param) => await ExecuteSearchAsync(param as UserData));
        }
        private static bool IsValidField(string field) => !string.IsNullOrEmpty(field) && !string.Equals(field, "NULL", StringComparison.OrdinalIgnoreCase);
        public async Task<string> ExecuteSearchAsync(UserData userData)
        {
            if (userData == null) return "User data is null.";
            if (!userData.DehashedChecked) return "Dehashed search not enabled.";

            var taskResultsWithTitles = BuildDehashedTasksWithTitles(userData);
            var results = await Task.WhenAll(taskResultsWithTitles);

            Entries.Clear();
            FormattedEntries.Clear();
            StringBuilder resultsSummary = new StringBuilder();

            foreach (var resultWithTitle in results)
            {
                if (!string.IsNullOrWhiteSpace(resultWithTitle.Title))
                {
                    resultsSummary.AppendLine(resultWithTitle.Title);
                }

                foreach (var result in resultWithTitle.Results)
                {

                    Entries.Add(result);

                    List<string> nonEmptyFields = new List<string>();
                    if (IsValidField(result.Id)) nonEmptyFields.Add($"ID: {result.Id}");
                    if (IsValidField(result.Email)) nonEmptyFields.Add($"Email: {result.Email}");
                    if (IsValidField(result.Ip_address)) nonEmptyFields.Add($"Ip_address: {result.Ip_address}");
                    if (IsValidField(result.Username)) nonEmptyFields.Add($"Username: {result.Username}");
                    if (IsValidField(result.Password)) nonEmptyFields.Add($"Password: {result.Password}");
                    if (IsValidField(result.Hashed_password)) nonEmptyFields.Add($"Hashed_password: {result.Hashed_password}");
                    if (IsValidField(result.Hash_type)) nonEmptyFields.Add($"Hash_type: {result.Hash_type}");
                    if (IsValidField(result.Name)) nonEmptyFields.Add($"Name: {result.Name}");
                    if (IsValidField(result.Vin)) nonEmptyFields.Add($"Vin: {result.Vin}");
                    if (IsValidField(result.Address)) nonEmptyFields.Add($"Address: {result.Address}");
                    if (IsValidField(result.Phone)) nonEmptyFields.Add($"Phone: {result.Phone}");
                    if (IsValidField(result.Database_name)) nonEmptyFields.Add($"Database_name: {result.Database_name}");

                    string formattedEntry = string.Join(", ", nonEmptyFields);
                    if (!string.IsNullOrEmpty(formattedEntry))
                    {
                        FormattedEntries.Add(formattedEntry);
                        resultsSummary.AppendLine(formattedEntry);
                    }
                }
            }
            return resultsSummary.Length > 0 ? resultsSummary.ToString() : "";
        }
        private IEnumerable<Task<TaskResultWithTitle>> BuildDehashedTasksWithTitles(UserData userData)
        {
            // defines dictionary which writes titles of searches before them
            var tasks = new List<Task<TaskResultWithTitle>>();

            var fieldMap = new Dictionary<string, (string value, string title)>
    {
        { "Id", (userData.Id, "Results for Id:") },
        { "Email", (userData.Email, "Results for Email:") },
        { "IpAddress", (userData.IpAddress, "Results for IpAddress:") },
        { "Username", (userData.Username, "Results for Username:") },
        { "Password", (userData.Password, "Results for Password:") },
        { "HashedPassword", (userData.HashedPassword, "Results for HashedPassword:") },
        { "HashType", (userData.HashType, "Results for HashType:") },
        { "Name", (userData.Name, "Results for Name:") },
        { "Vin", (userData.Vin, "Results for Vin:") },
        { "Address", (userData.Address, "Results for Address:") },
        { "Phone", (userData.Phone, "Results for Phone:") },
        { "DatabaseName", (userData.DatabaseName, "Results for DatabaseName:") },
    };

            foreach (var field in fieldMap)
            {
                if (!string.IsNullOrWhiteSpace(field.Value.value))
                {
                    string queryValue = field.Key.ToLowerInvariant() + ":\"" + field.Value.value + "\"";
                    var task = dehashedService.CheckIfDataHasBeenLeaked(queryValue)
                        .ContinueWith(t => new TaskResultWithTitle
                        {
                            Title = field.Value.title + " " + field.Value.value,
                            Results = t.Result
                        });
                    tasks.Add(task);
                }
            }

            return tasks;
        }
        private IEnumerable<Task<IEnumerable<Dehashed.Entry>>> BuildDehashedTasks(UserData userData)
        {
            var tasks = new List<Task<IEnumerable<Dehashed.Entry>>>();

            if (!string.IsNullOrWhiteSpace(userData.Email))
            {
                var task = dehashedService.CheckIfDataHasBeenLeaked($"email:\"{userData.Email}\"").ContinueWith(t => t.Result.AsEnumerable());
                tasks.Add(task);
            }
            if (!string.IsNullOrWhiteSpace(userData.Username))
            {
                var task = dehashedService.CheckIfDataHasBeenLeaked($"username:\"{userData.Username}\"").ContinueWith(t => t.Result.AsEnumerable());
                tasks.Add(task);
            }
            if (!string.IsNullOrWhiteSpace(userData.Password))
            {
                var task = dehashedService.CheckIfDataHasBeenLeaked($"password:\"{userData.Password}\"").ContinueWith(t => t.Result.AsEnumerable());
                tasks.Add(task);
            }
            if (!string.IsNullOrWhiteSpace(userData.Phone))
            {
                var task = dehashedService.CheckIfDataHasBeenLeaked($"phone:\"{userData.Phone}\"").ContinueWith(t => t.Result.AsEnumerable());
                tasks.Add(task);
            }
            if (!string.IsNullOrWhiteSpace(userData.Address))
            {
                var task = dehashedService.CheckIfDataHasBeenLeaked($"address:\"{userData.Address}\"").ContinueWith(t => t.Result.AsEnumerable());
                tasks.Add(task);
            }
            if (!string.IsNullOrWhiteSpace(userData.HashedPassword))
            {
                var task = dehashedService.CheckIfDataHasBeenLeaked($"hashedPassword:\"{userData.HashedPassword}\"").ContinueWith(t => t.Result.AsEnumerable());
                tasks.Add(task);
            }
            if (!string.IsNullOrWhiteSpace(userData.HashType))
            {
                var task = dehashedService.CheckIfDataHasBeenLeaked($"hashType:\"{userData.HashType}\"").ContinueWith(t => t.Result.AsEnumerable());
                tasks.Add(task);
            }
            if (!string.IsNullOrWhiteSpace(userData.Name))
            {
                var task = dehashedService.CheckIfDataHasBeenLeaked($"name:\"{userData.Name}\"").ContinueWith(t => t.Result.AsEnumerable());
                tasks.Add(task);
            }
            if (!string.IsNullOrWhiteSpace(userData.Id))
            {
                var task = dehashedService.CheckIfDataHasBeenLeaked($"id:\"{userData.Id}\"").ContinueWith(t => t.Result.AsEnumerable());
                tasks.Add(task);
            }
            if (!string.IsNullOrWhiteSpace(userData.IpAddress))
            {
                var task = dehashedService.CheckIfDataHasBeenLeaked($"ipAddress:\"{userData.IpAddress}\"").ContinueWith(t => t.Result.AsEnumerable());
                tasks.Add(task);
            }
            if (!string.IsNullOrWhiteSpace(userData.Vin))
            {
                var task = dehashedService.CheckIfDataHasBeenLeaked($"vin:\"{userData.Vin}\"").ContinueWith(t => t.Result.AsEnumerable());
                tasks.Add(task);
            }
            if (!string.IsNullOrWhiteSpace(userData.DatabaseName))
            {
                var task = dehashedService.CheckIfDataHasBeenLeaked($"databaseName:\"{userData.DatabaseName}\"").ContinueWith(t => t.Result.AsEnumerable());
                tasks.Add(task);
            }
            return tasks;
        }
    }
    public class TaskResultWithTitle
    {
        public string Title { get; set; }
        public IEnumerable<Dehashed.Entry> Results { get; set; }
    }
}