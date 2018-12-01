using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PathCleaner
{
    public partial class MainForm : Form
    {
        public MainForm() => InitializeComponent();

        public static Result DuplicatePathChecker(string folder, Maybe<string> previousFolder)
        {
            if (previousFolder.HasValue
                && String.Compare(folder, previousFolder.Value, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return Result.Fail("Duplicate");
            }

                return Result.Ok();


        }

        public static Result MissingPathChecker(string folder, Maybe<string> previousFolder)
        {
            if (!Directory.Exists(folder))
            {
                return Result.Fail("Missing");
            }

            return Result.Ok();

        }

        public static Result EmptyPathChecker(string folder, Maybe<string> previousFolder)
        {
            try
            { 

                if (!Directory.EnumerateFiles(folder).Any())
                {
                    return Result.Fail("Empty");
                }

                return Result.Ok();
            }
            catch
            {
                return Result.Fail("Empty");
            }
        }

        private static readonly HashSet<string> executableExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".exe", ".com", ".bat", ".cmd", ".ps1", ".dll"
        };

        public static Result NoExecutablesPathChecker(string folder, Maybe<string> previousFolder)
        {
            try
            { 

                if (!Directory.EnumerateFiles(folder)
                    .Where(s => executableExtensions.Contains(System.IO.Path.GetExtension(s)))
                    .Any())
                {
                    return Result.Fail("No executables");
                }

                return Result.Ok();

            }
            catch
            {
                return Result.Fail("No executables");

            }
        }


        private List<Func<string, Maybe<string>, Result>> pathCheckers = new List<Func<string, Maybe<string>, Result>>()
        {
            DuplicatePathChecker,
            MissingPathChecker,
            EmptyPathChecker,
            NoExecutablesPathChecker,
        };

        private void MainForm_Load(object sender, EventArgs e) => findProblematicItems();

        private void findProblematicItems()
        {
            UseWaitCursor = true;
            problemList.Items.Clear();

            var k = Extensions.GetPathVariable().ToPath();

            var p = ProblemIdentifier.FindProblems(pathCheckers, k);

            p.ForEach(x =>
            {
                var listItem = new ListViewItem(x.Path);
                listItem.SubItems.Add(new ListViewItem.ListViewSubItem(listItem, x.Reason));
                problemList.Items.Add(listItem);

            });


            UseWaitCursor = false;
        }

        private void cleanupButton_Click(object sender, EventArgs e)
        {
            var path = Extensions.GetPathVariable().ToPath();

            var k = problemList.SelectedItems.ToList();

            path.Folders.Where(x => k.Contains(x)).ToList().ForEach(x => path.Folders.Remove(x));

            path.UpdatePathVariable();

            findProblematicItems();
        }
    }
}