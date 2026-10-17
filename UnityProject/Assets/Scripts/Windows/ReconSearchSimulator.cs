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

        if (targetsDomain &&
            targetsPdf &&
            targetsConfidential)
        {
            return
                "Search Results\n\n" +

                "Northstar Technologies - Employee Handbook\n" +
                "northstar.test/documents/employee-handbook.pdf\n\n" +

                "CONFIDENTIAL - Network Infrastructure Overview\n" +
                "northstar.test/internal/network-infrastructure.pdf\n" +
                "Publicly indexed document containing internal infrastructure information.\n\n" +

                "Northstar Technologies - Benefits Guide\n" +
                "northstar.test/documents/benefits.pdf";
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
}