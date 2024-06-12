# Slack Data Anonymizer

## Overview
Slack Data Anonymizer is a dotnet CLI tool designed to anonymize data from Slack. This is useful for preserving privacy when sharing or analyzing Slack data.

## Features
- Anonymizes user data in Slack exports
- Retains message content while masking user identities
- Contains a functional `--help` section for command-line guidance

## Installation
1. Clone the repository:
    ```bash
    git clone https://github.com/abinkowski94/slack-data-anonymizer.git
    ```
2. Navigate to the project directory:
    ```bash
    cd slack-data-anonymizer
    ```

## Usage
1. Place your Slack export data in the `input` directory.
2. Run the anonymizer script:
    ```bash
    dotnet run --project src/SlackDataAnonymizer
    ```
3. Anonymized data will be available in the `output` directory.
4. For command-line options, use the `--help` flag:
    ```bash
    dotnet run --project src/SlackDataAnonymizer --help
    ```

## Contributing
Contributions are welcome! Please fork the repository and create a pull request with your changes.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact
For questions or suggestions, please open an issue in this repository.
