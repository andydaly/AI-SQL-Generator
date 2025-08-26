# AI‑SQL‑Generator

*Generate SQL queries using natural language prompts and database connection details, powered by AI.*

---

##  Demo

![App screenshot1](AI-SQL-Generator/images/AISQLGeneratorScreenshot.png)

![App screenshot1](AI-SQL-Generator/images/ShowResultsScreenshot.png)

---

##  Description

**AI‑SQL‑Generator** is a tool designed to let users write plain‑English prompts, connect their database, and receive AI‑generated SQL queries based on the schema and prompt. Ideal for data analysts, developers, or anyone needing SQL quickly without deep SQL expertise.

---

##  Features

- Generate SQL from natural language prompts  
- Supports multiple database types (based on connection strings)  
- Automatically includes DB connection details from `appsettings.json`  
- Helps users learn SQL via transparent query generation  
- Minimal setup—just add your credentials and you're ready

---

##  Getting Started

### Prerequisites

- .NET SDK (matching the project's framework)  
- An active AI model endpoint (Azure OpenAI or Azure OpenAI-compatible)  
- Valid connection string for your SQL database

### Setup

1. Clone the repo  
   ```bash
   git clone https://github.com/andydaly/AI-SQL-Generator.git
   ```

2. Open the solution (e.g., in Visual Studio) and navigate to the **AI‑SQL‑Form** project.

3. Create an `appsettings.json` in the `AI‑SQL‑Form` project directory with the following content:

   ```json
   {
     "AI": {
       "Endpoint": "<your AI endpoint URL>",
       "DeploymentName": "<AI model deployment name>",
       "ApiKey": "<your API key>"
     },
     "DefaultConnection": "<your DB connection string>"
   }
   ```
![Description](AI-SQL-Generator/images/AppSettingsInstructions.png)

4. In the `appsettings.json` file's Properties, set **Copy to Output Directory** to **Copy always**.

5. Run the **AI‑SQL‑Form** project. You’ll see a form where you can:
   - Review or override connection details  
   - Enter your natural language prompt  
   - Submit to generate SQL  

---

##  Usage Example

- Prompt:  
  `List all users who joined in the last 30 days and have made at least one purchase.`

- The app uses AI (via your endpoint) and your default connection settings to generate a matching SQL query. The output is visible and editable for your own learning and use.

---

##  FAQs

**Q: What AI services work with this?**  
A: Any service that provides REST endpoints with Azure-compatible OpenAI interface—configure in `appsettings.json`.

**Q: What databases are supported?**  
A: Any database with a valid .NET connection string (e.g., SQL Server, Postgres, SQLite).

**Q: Can I use without configuring `DefaultConnection`?**  
A: Yes—just leave it blank and manually enter DB connection details via the form before hitting **Generate**.

---

##  Contributing

Contributions are welcome! Please create issues or PRs for new features, bug fixes, or suggestions. Label them clearly (e.g., `enhancement`, `bug`, `docs`).


