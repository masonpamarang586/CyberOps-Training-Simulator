public class ReconSearchSimulator
{
    public string Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return "Enter a search query.";
        }

        string normalized =
            query.Trim().ToLowerInvariant();

        bool targetsDomain =
            normalized.Contains("site:northstar.test");

        bool targetsPdf =
            normalized.Contains("filetype:pdf");

        bool targetsConfidential =
            normalized.Contains("\"confidential\"") ||
            normalized.Contains("confidential");

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
                "northstar.test/documents/benefits.pdf\n\n" +

                "Several additional PDF documents were indexed.";
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
                "northstar.test/documents\n\n" +

                "Try narrowing the search using additional operators.";
        }

        return
            "Search Results\n\n" +

            "Thousands of results found.\n\n" +

            "Your search is too broad to identify useful information efficiently.";
    }

    public bool RevealsExposedDocument(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return false;
        }

        string normalized =
            query.Trim().ToLowerInvariant();

        return
            normalized.Contains("site:northstar.test") &&
            normalized.Contains("filetype:pdf") &&
            normalized.Contains("confidential");
    }
}