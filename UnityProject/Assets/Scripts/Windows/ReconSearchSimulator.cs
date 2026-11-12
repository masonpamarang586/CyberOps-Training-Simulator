public enum ReconSearchProfile
{
    PublicExposure,
    ExposedServiceAccount
}

public class ReconSearchSimulator
{
    private readonly ReconSearchProfile profile;

    public ReconSearchSimulator(
        ReconSearchProfile profile)
    {
        this.profile = profile;
    }

    public string Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return "Enter a search query.";
        }

        switch (profile)
        {
            case ReconSearchProfile.ExposedServiceAccount:
                return SearchServiceAccount(query);

            default:
                return SearchPublicExposure(query);
        }
    }

    public bool RevealsExposedDocument(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return false;
        }

        string normalized =
            query.Trim().ToLowerInvariant();

        switch (profile)
        {
            case ReconSearchProfile.ExposedServiceAccount:
                return
                    normalized.Contains("site:northstar.test") &&
                    normalized.Contains("filetype:txt") &&
                    normalized.Contains("service account");

            default:
                return
                    normalized.Contains("site:northstar.test") &&
                    normalized.Contains("filetype:pdf") &&
                    normalized.Contains("confidential");
        }
    }

    private string SearchServiceAccount(string query)
    {
        string normalized =
            query.Trim().ToLowerInvariant();

        bool targetsDomain =
            normalized.Contains("site:northstar.test");

        bool targetsTxt =
            normalized.Contains("filetype:txt");

        bool targetsServiceAccount =
            normalized.Contains("service account");

        if (RevealsExposedDocument(query))
        {
            return
                "Search Results\n\n" +
                "Northstar Technologies - IT Support Notes\n" +
                "northstar.test/support/it-notes.txt";
        }

        if (targetsDomain && targetsTxt)
        {
            return
                "Search Results\n\n" +
                "Northstar Technologies - IT Support Notes\n" +
                "northstar.test/support/it-notes.txt\n\n" +
                "Backup Migration Checklist\n" +
                "northstar.test/internal/backup-migration.txt\n\n" +
                "Several additional text documents were indexed.";
        }

        if (targetsDomain)
        {
            return
                "Search Results\n\n" +
                "Northstar Technologies\n" +
                "northstar.test\n\n" +
                "Northstar Technologies - Support\n" +
                "northstar.test/support\n\n" +
                "Northstar Technologies - Internal Documents\n" +
                "northstar.test/internal\n\n" +
                "Try narrowing the search using additional operators.";
        }

        return
            "Search Results\n\n" +
            "Thousands of results found.\n\n" +
            "Your search is too broad to identify useful information efficiently.";
    }

    private string SearchPublicExposure(string query)
    {
        string normalized =
            query.Trim().ToLowerInvariant();

        bool targetsDomain =
            normalized.Contains("site:northstar.test");

        bool targetsPdf =
            normalized.Contains("filetype:pdf");

        if (RevealsExposedDocument(query))
        {
            return
                "Search Results\n\n" +
                "Northstar Technologies - Employee Handbook\n" +
                "northstar.test/documents/employee-handbook.pdf";
        }

        if (targetsDomain && targetsPdf)
        {
            return
                "Search Results\n\n" +
                "Northstar Technologies - Employee Handbook\n" +
                "northstar.test/documents/employee-handbook.pdf\n\n" +
                "Quarterly Operations Report\n" +
                "northstar.test/reports/q3-operations.pdf\n\n" +
                "Benefits Guide\n" +
                "northstar.test/documents/benefits.pdf";
        }

        if (targetsDomain)
        {
            return
                "Search Results\n\n" +
                "Northstar Technologies\n" +
                "northstar.test\n\n" +
                "Northstar Technologies - Careers\n" +
                "northstar.test/careers\n\n" +
                "Northstar Technologies - Documents\n" +
                "northstar.test/documents";
        }

        return
            "Search Results\n\n" +
            "Thousands of results found.\n\n" +
            "Your search is too broad to identify useful information efficiently.";
    }
}