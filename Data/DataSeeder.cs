using IncidentResponse.Domain.Entities;
using IncidentResponse.Domain.Enums;

namespace IncidentResponse.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (db.Users.Any() && db.Cases.Any() && db.Alerts.Any()) return;

        // Seed users
        var users = new List<AppUser>
        {
            new() { Id = Guid.NewGuid(), Username = "admin", DisplayName = "Administrator", Role = "Admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"), CreatedAt = DateTime.UtcNow.AddDays(-30) },
            new() { Id = Guid.NewGuid(), Username = "analyst1", DisplayName = "Alice Chen", Role = "Analyst",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test@123"), CreatedAt = DateTime.UtcNow.AddDays(-20) },
            new() { Id = Guid.NewGuid(), Username = "analyst2", DisplayName = "Bob Martinez", Role = "Analyst",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test@123"), CreatedAt = DateTime.UtcNow.AddDays(-20) },
            new() { Id = Guid.NewGuid(), Username = "manager1", DisplayName = "Carol White", Role = "Manager",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test@123"), CreatedAt = DateTime.UtcNow.AddDays(-25) },
            new() { Id = Guid.NewGuid(), Username = "exec1", DisplayName = "David Kim", Role = "Executive",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test@123"), CreatedAt = DateTime.UtcNow.AddDays(-25) }
        };
        db.Users.AddRange(users);

        // Seed incidents
        var now = DateTime.UtcNow;

        var inc1 = new Incident
        {
            Id = Guid.NewGuid(),
            IncidentNumber = "INC-2024-001",
            Title = "Suspected Ransomware - Accounting Cluster",
            Description = "Multiple endpoints in the accounting department have been encrypted. Ransom note found on shared drives. Potential for widespread data loss if not contained immediately.",
            Severity = Severity.SEV1,
            Status = IncidentStatus.Containment,
            AssignedTo = "analyst1",
            DetectedAt = now.AddHours(-6),
            ContainedAt = now.AddHours(-2),
            UpdatedAt = now.AddHours(-1),
            MitreTactics = "[\"Execution\",\"Persistence\",\"Impact\"]",
            Tags = "[\"ransomware\",\"encryption\",\"accounting\"]",
            CreatedBy = "analyst1"
        };

        var inc2 = new Incident
        {
            Id = Guid.NewGuid(),
            IncidentNumber = "INC-2024-002",
            Title = "Phishing Campaign - Finance Dept",
            Description = "A targeted spear-phishing campaign has been identified targeting the Finance department. Several employees have clicked malicious links. Credential harvesting suspected.",
            Severity = Severity.SEV2,
            Status = IncidentStatus.Investigation,
            AssignedTo = "analyst2",
            DetectedAt = now.AddHours(-18),
            UpdatedAt = now.AddHours(-3),
            MitreTactics = "[\"Initial Access\",\"Credential Access\",\"Collection\"]",
            Tags = "[\"phishing\",\"finance\",\"credentials\"]",
            CreatedBy = "analyst2"
        };

        var inc3 = new Incident
        {
            Id = Guid.NewGuid(),
            IncidentNumber = "INC-2024-003",
            Title = "Unauthorized Access - Domain Controller",
            Description = "Unauthorized administrative access detected on the primary Domain Controller. Multiple failed attempts followed by a successful login from an unrecognized IP address at 03:42 UTC.",
            Severity = Severity.SEV0,
            Status = IncidentStatus.Triage,
            AssignedTo = "analyst1",
            DetectedAt = now.AddHours(-2),
            UpdatedAt = now.AddMinutes(-30),
            MitreTactics = "[\"Initial Access\",\"Privilege Escalation\",\"Lateral Movement\",\"Discovery\"]",
            Tags = "[\"unauthorized-access\",\"domain-controller\",\"admin\"]",
            CreatedBy = "admin"
        };

        var inc4 = new Incident
        {
            Id = Guid.NewGuid(),
            IncidentNumber = "INC-2024-004",
            Title = "Data Exfiltration Alert - HR Systems",
            Description = "Unusual outbound data transfer detected from HR database servers. Large volumes of employee PII may have been exfiltrated via a compromised service account.",
            Severity = Severity.SEV1,
            Status = IncidentStatus.Eradication,
            AssignedTo = "analyst2",
            DetectedAt = now.AddDays(-2),
            ContainedAt = now.AddDays(-1).AddHours(-12),
            UpdatedAt = now.AddHours(-4),
            MitreTactics = "[\"Collection\",\"Exfiltration\",\"Defense Evasion\"]",
            Tags = "[\"exfiltration\",\"pii\",\"hr\"]",
            CreatedBy = "analyst1"
        };

        var inc5 = new Incident
        {
            Id = Guid.NewGuid(),
            IncidentNumber = "INC-2024-005",
            Title = "Malware Detected - Endpoint Fleet",
            Description = "EDR alerts triggered across 12 workstations in Building A. Identified as Emotet variant dropper. All affected machines have been isolated and are being reimaged.",
            Severity = Severity.SEV2,
            Status = IncidentStatus.Closed,
            AssignedTo = "analyst1",
            DetectedAt = now.AddDays(-5),
            ContainedAt = now.AddDays(-4).AddHours(-20),
            ResolvedAt = now.AddDays(-3),
            UpdatedAt = now.AddDays(-3),
            MitreTactics = "[\"Execution\",\"Persistence\",\"Defense Evasion\"]",
            Tags = "[\"malware\",\"emotet\",\"endpoints\"]",
            CreatedBy = "analyst2"
        };

        var inc6 = new Incident
        {
            Id = Guid.NewGuid(),
            IncidentNumber = "INC-2024-006",
            Title = "Lateral Movement Detected - Dev Network",
            Description = "Suspicious lateral movement observed in the Dev network. An attacker appears to be using pass-the-hash techniques to move between developer workstations and CI/CD servers.",
            Severity = Severity.SEV1,
            Status = IncidentStatus.Investigation,
            AssignedTo = "analyst2",
            DetectedAt = now.AddHours(-10),
            UpdatedAt = now.AddHours(-2),
            MitreTactics = "[\"Lateral Movement\",\"Credential Access\",\"Discovery\",\"Privilege Escalation\"]",
            Tags = "[\"lateral-movement\",\"pass-the-hash\",\"devops\"]",
            CreatedBy = "admin"
        };

        var inc7 = new Incident
        {
            Id = Guid.NewGuid(),
            IncidentNumber = "INC-2024-007",
            Title = "Insider Threat - Sensitive Data Access",
            Description = "Anomalous access patterns detected for a privileged user account. The account accessed sensitive financial records outside business hours and attempted to copy files to removable media.",
            Severity = Severity.SEV2,
            Status = IncidentStatus.Recovery,
            AssignedTo = "manager1",
            DetectedAt = now.AddDays(-3),
            ContainedAt = now.AddDays(-2).AddHours(-18),
            UpdatedAt = now.AddDays(-1),
            MitreTactics = "[\"Collection\",\"Exfiltration\",\"Defense Evasion\"]",
            Tags = "[\"insider-threat\",\"data-access\",\"privileged-user\"]",
            CreatedBy = "manager1"
        };

        var inc8 = new Incident
        {
            Id = Guid.NewGuid(),
            IncidentNumber = "INC-2024-008",
            Title = "DDoS Attack - Customer Portal",
            Description = "Distributed denial-of-service attack targeting the customer-facing web portal. Traffic analysis shows amplification attack from multiple botnets. CDN mitigation partially effective.",
            Severity = Severity.SEV3,
            Status = IncidentStatus.Closed,
            AssignedTo = "analyst1",
            DetectedAt = now.AddDays(-7),
            ContainedAt = now.AddDays(-7).AddHours(-16),
            ResolvedAt = now.AddDays(-6),
            UpdatedAt = now.AddDays(-6),
            MitreTactics = "[\"Impact\"]",
            Tags = "[\"ddos\",\"availability\",\"portal\"]",
            CreatedBy = "analyst2"
        };

        var incidents = new List<Incident> { inc1, inc2, inc3, inc4, inc5, inc6, inc7, inc8 };
        db.Incidents.AddRange(incidents);

        // Notes
        var notes = new List<IncidentNote>
        {
            // INC-001 notes
            new() { Id = Guid.NewGuid(), IncidentId = inc1.Id, Author = "analyst1", CreatedAt = now.AddHours(-5.5),
                Content = "Initial triage complete. 14 endpoints affected in the accounting VLAN. Encryption appears to use AES-256. Network segment has been isolated at the switch level." },
            new() { Id = Guid.NewGuid(), IncidentId = inc1.Id, Author = "manager1", CreatedAt = now.AddHours(-4),
                Content = "Management notified. Legal and compliance teams alerted per incident response policy. Ransomware sample has been submitted to our AV vendor for analysis." },
            new() { Id = Guid.NewGuid(), IncidentId = inc1.Id, Author = "analyst1", CreatedAt = now.AddHours(-2),
                Content = "Identified patient zero: acct-ws-07. User received malicious macro-embedded Excel file via email. Email gateway logs pulled for forensics. EDR telemetry confirms initial execution at 08:14 UTC." },

            // INC-002 notes
            new() { Id = Guid.NewGuid(), IncidentId = inc2.Id, Author = "analyst2", CreatedAt = now.AddHours(-16),
                Content = "Phishing email originated from spoofed domain finance-docs-portal.com. 23 employees received the email, 4 clicked the link. Credential reset initiated for affected accounts." },
            new() { Id = Guid.NewGuid(), IncidentId = inc2.Id, Author = "analyst2", CreatedAt = now.AddHours(-8),
                Content = "MFA bypass attempted on 2 accounts using harvested credentials. Both attempts failed due to MFA enforcement. Accounts locked as precaution. Investigating for any successful pre-MFA access." },
            new() { Id = Guid.NewGuid(), IncidentId = inc2.Id, Author = "manager1", CreatedAt = now.AddHours(-3),
                Content = "IT Security has blocked the phishing domain and all related IP ranges at the perimeter. Awareness communication sent to all Finance staff. Monitoring for further phishing attempts." },

            // INC-003 notes
            new() { Id = Guid.NewGuid(), IncidentId = inc3.Id, Author = "admin", CreatedAt = now.AddHours(-1.8),
                Content = "Critical: Unauthorized administrative session on DC01. Source IP 185.234.219.47 (Tor exit node). Session lasted 4 minutes. AD audit logs show enumeration of privileged groups and GPO access." },
            new() { Id = Guid.NewGuid(), IncidentId = inc3.Id, Author = "analyst1", CreatedAt = now.AddHours(-1),
                Content = "Reviewing AD change logs. No new accounts created, no GPO modifications detected. Possible reconnaissance. Admin credentials have been rotated. SIEM alerts configured for further DC activity." },

            // INC-004 notes
            new() { Id = Guid.NewGuid(), IncidentId = inc4.Id, Author = "analyst1", CreatedAt = now.AddDays(-2).AddHours(2),
                Content = "Service account svc_hrbackup was used to execute bulk SQL exports. Data volume: ~4.2GB over 3 hours via HTTPS to external cloud storage endpoint. Account disabled immediately." },
            new() { Id = Guid.NewGuid(), IncidentId = inc4.Id, Author = "analyst2", CreatedAt = now.AddDays(-1).AddHours(-20),
                Content = "Forensic analysis of the service account confirms it was compromised via a vulnerable HR web application. CVE-2023-44487 exploited. Patch applied, endpoint rebuilt from golden image." },
            new() { Id = Guid.NewGuid(), IncidentId = inc4.Id, Author = "manager1", CreatedAt = now.AddDays(-1),
                Content = "DPA notification initiated as PII of 847 employees may have been exfiltrated. Legal team engaged. Affected employees will be notified per GDPR requirements within 72-hour window." },

            // INC-005 notes
            new() { Id = Guid.NewGuid(), IncidentId = inc5.Id, Author = "analyst2", CreatedAt = now.AddDays(-4).AddHours(-22),
                Content = "Emotet dropper deployed via malicious OneNote attachment. EDR quarantined payloads on all 12 machines. C2 communication to 192.168.89.2 (internal pivot) and 45.33.32.156 (external)." },
            new() { Id = Guid.NewGuid(), IncidentId = inc5.Id, Author = "analyst1", CreatedAt = now.AddDays(-3).AddHours(-12),
                Content = "All 12 workstations reimaged. New machine certificates issued. User passwords reset. Checked for any lateral movement to servers - none found. Incident contained." },

            // INC-006 notes
            new() { Id = Guid.NewGuid(), IncidentId = inc6.Id, Author = "analyst2", CreatedAt = now.AddHours(-9),
                Content = "Pass-the-hash activity confirmed. Attacker harvested NTLM hashes from dev-ws-03 and used them to authenticate to build-server-01 and artifact-repo-02. Affected systems isolated." },
            new() { Id = Guid.NewGuid(), IncidentId = inc6.Id, Author = "analyst2", CreatedAt = now.AddHours(-5),
                Content = "Investigation of build-server-01 shows no pipeline tampering detected. However, attacker had read access to environment variables for 47 minutes. Rotation of all CI/CD secrets initiated." },
            new() { Id = Guid.NewGuid(), IncidentId = inc6.Id, Author = "manager1", CreatedAt = now.AddHours(-2),
                Content = "DevOps team notified. All CI/CD pipeline secrets rotated. MFA enforced for all DevOps tooling. Reviewing if any secrets were used externally during the exposure window." },

            // INC-007 notes
            new() { Id = Guid.NewGuid(), IncidentId = inc7.Id, Author = "manager1", CreatedAt = now.AddDays(-3).AddHours(1),
                Content = "UEBA alert triggered for user j.thompson. 3 off-hours access events to sensitive financial records. DLP alert for USB write attempt at 23:18. HR and Legal notified. Account suspended pending investigation." },
            new() { Id = Guid.NewGuid(), IncidentId = inc7.Id, Author = "analyst2", CreatedAt = now.AddDays(-2),
                Content = "Forensic image of user's workstation completed. Found evidence of data staging in a hidden folder. 234 files copied to external drive. Interview with user scheduled by HR." },

            // INC-008 notes
            new() { Id = Guid.NewGuid(), IncidentId = inc8.Id, Author = "analyst1", CreatedAt = now.AddDays(-7).AddHours(2),
                Content = "Peak traffic: 1.2Tbps. DNS amplification attack vector confirmed. CDN absorbing ~80% of traffic. Customer portal intermittent - response times degraded. ISP contacted for upstream filtering." },
            new() { Id = Guid.NewGuid(), IncidentId = inc8.Id, Author = "analyst1", CreatedAt = now.AddDays(-6).AddHours(-2),
                Content = "Attack traffic subsided after 8 hours. ISP implemented RTBH filtering for attack source ranges. Portal fully restored. Post-incident review scheduled. Recommend WAF rule tightening." }
        };
        db.IncidentNotes.AddRange(notes);

        // Affected Assets
        var assets = new List<AffectedAsset>
        {
            // INC-001 assets
            new() { Id = Guid.NewGuid(), IncidentId = inc1.Id, Hostname = "acct-ws-07", IpAddress = "10.10.5.107", AssetType = "Workstation", AssetStatus = "Isolated", RiskScore = 95 },
            new() { Id = Guid.NewGuid(), IncidentId = inc1.Id, Hostname = "acct-ws-12", IpAddress = "10.10.5.112", AssetType = "Workstation", AssetStatus = "Isolated", RiskScore = 88 },
            new() { Id = Guid.NewGuid(), IncidentId = inc1.Id, Hostname = "acct-srv-01", IpAddress = "10.10.5.20", AssetType = "Server", AssetStatus = "Isolated", RiskScore = 92 },
            new() { Id = Guid.NewGuid(), IncidentId = inc1.Id, Hostname = "acct-fs-01", IpAddress = "10.10.5.21", AssetType = "Server", AssetStatus = "Active", RiskScore = 75 },

            // INC-002 assets
            new() { Id = Guid.NewGuid(), IncidentId = inc2.Id, Hostname = "fin-ws-03", IpAddress = "10.10.6.103", AssetType = "Workstation", AssetStatus = "Active", RiskScore = 60 },
            new() { Id = Guid.NewGuid(), IncidentId = inc2.Id, Hostname = "fin-ws-08", IpAddress = "10.10.6.108", AssetType = "Workstation", AssetStatus = "Active", RiskScore = 55 },

            // INC-003 assets
            new() { Id = Guid.NewGuid(), IncidentId = inc3.Id, Hostname = "dc-01", IpAddress = "10.1.1.1", AssetType = "Server", AssetStatus = "Active", RiskScore = 99 },
            new() { Id = Guid.NewGuid(), IncidentId = inc3.Id, Hostname = "dc-02", IpAddress = "10.1.1.2", AssetType = "Server", AssetStatus = "Active", RiskScore = 90 },

            // INC-004 assets
            new() { Id = Guid.NewGuid(), IncidentId = inc4.Id, Hostname = "hr-db-01", IpAddress = "10.10.7.10", AssetType = "Server", AssetStatus = "Isolated", RiskScore = 97 },
            new() { Id = Guid.NewGuid(), IncidentId = inc4.Id, Hostname = "hr-app-01", IpAddress = "10.10.7.11", AssetType = "Server", AssetStatus = "Reimaged", RiskScore = 30 },
            new() { Id = Guid.NewGuid(), IncidentId = inc4.Id, Hostname = "hr-ws-04", IpAddress = "10.10.7.104", AssetType = "Workstation", AssetStatus = "Reimaged", RiskScore = 20 },

            // INC-005 assets
            new() { Id = Guid.NewGuid(), IncidentId = inc5.Id, Hostname = "bldga-ws-01", IpAddress = "10.20.1.101", AssetType = "Workstation", AssetStatus = "Reimaged", RiskScore = 10 },
            new() { Id = Guid.NewGuid(), IncidentId = inc5.Id, Hostname = "bldga-ws-05", IpAddress = "10.20.1.105", AssetType = "Workstation", AssetStatus = "Reimaged", RiskScore = 10 },
            new() { Id = Guid.NewGuid(), IncidentId = inc5.Id, Hostname = "bldga-ws-09", IpAddress = "10.20.1.109", AssetType = "Workstation", AssetStatus = "Reimaged", RiskScore = 10 },

            // INC-006 assets
            new() { Id = Guid.NewGuid(), IncidentId = inc6.Id, Hostname = "dev-ws-03", IpAddress = "10.30.1.103", AssetType = "Workstation", AssetStatus = "Isolated", RiskScore = 82 },
            new() { Id = Guid.NewGuid(), IncidentId = inc6.Id, Hostname = "build-server-01", IpAddress = "10.30.1.20", AssetType = "Server", AssetStatus = "Isolated", RiskScore = 88 },
            new() { Id = Guid.NewGuid(), IncidentId = inc6.Id, Hostname = "artifact-repo-02", IpAddress = "10.30.1.22", AssetType = "Server", AssetStatus = "Active", RiskScore = 70 },

            // INC-007 assets
            new() { Id = Guid.NewGuid(), IncidentId = inc7.Id, Hostname = "fin-ws-11", IpAddress = "10.10.6.111", AssetType = "Workstation", AssetStatus = "Isolated", RiskScore = 65 },
            new() { Id = Guid.NewGuid(), IncidentId = inc7.Id, Hostname = "fin-db-02", IpAddress = "10.10.6.22", AssetType = "Server", AssetStatus = "Active", RiskScore = 40 },

            // INC-008 assets
            new() { Id = Guid.NewGuid(), IncidentId = inc8.Id, Hostname = "web-lb-01", IpAddress = "203.0.113.10", AssetType = "Network", AssetStatus = "Active", RiskScore = 45 },
            new() { Id = Guid.NewGuid(), IncidentId = inc8.Id, Hostname = "web-app-01", IpAddress = "203.0.113.11", AssetType = "Server", AssetStatus = "Active", RiskScore = 35 }
        };
        db.AffectedAssets.AddRange(assets);

        // IOCs
        var iocs = new List<IndicatorOfCompromise>
        {
            // INC-001 IOCs
            new() { Id = Guid.NewGuid(), IncidentId = inc1.Id, IocType = "Hash", Value = "a1b2c3d4e5f6789012345678901234567890abcd", Description = "Ransomware dropper SHA1", AddedAt = now.AddHours(-5) },
            new() { Id = Guid.NewGuid(), IncidentId = inc1.Id, IocType = "Domain", Value = "ransom-decrypt-help.onion.ws", Description = "Ransomware C2 / payment portal", AddedAt = now.AddHours(-4) },
            new() { Id = Guid.NewGuid(), IncidentId = inc1.Id, IocType = "IP", Value = "185.220.101.47", Description = "Ransomware C2 server IP", AddedAt = now.AddHours(-3) },

            // INC-002 IOCs
            new() { Id = Guid.NewGuid(), IncidentId = inc2.Id, IocType = "Domain", Value = "finance-docs-portal.com", Description = "Phishing domain", AddedAt = now.AddHours(-17) },
            new() { Id = Guid.NewGuid(), IncidentId = inc2.Id, IocType = "URL", Value = "https://finance-docs-portal.com/auth/verify", Description = "Credential harvesting URL", AddedAt = now.AddHours(-16) },
            new() { Id = Guid.NewGuid(), IncidentId = inc2.Id, IocType = "Email", Value = "noreply@finance-docs-portal.com", Description = "Phishing sender address", AddedAt = now.AddHours(-16) },

            // INC-003 IOCs
            new() { Id = Guid.NewGuid(), IncidentId = inc3.Id, IocType = "IP", Value = "185.234.219.47", Description = "Attacker source IP (Tor exit node)", AddedAt = now.AddHours(-1.9) },
            new() { Id = Guid.NewGuid(), IncidentId = inc3.Id, IocType = "Hash", Value = "b4d8c2e1f9a37056891234567890bcde12345678", Description = "Possible toolkit dropped on DC", AddedAt = now.AddHours(-0.8) },

            // INC-004 IOCs
            new() { Id = Guid.NewGuid(), IncidentId = inc4.Id, IocType = "Domain", Value = "backup-storage-cloud.io", Description = "External exfiltration endpoint", AddedAt = now.AddDays(-2).AddHours(3) },
            new() { Id = Guid.NewGuid(), IncidentId = inc4.Id, IocType = "IP", Value = "104.21.47.193", Description = "Cloud storage IP used for exfiltration", AddedAt = now.AddDays(-2).AddHours(3) },
            new() { Id = Guid.NewGuid(), IncidentId = inc4.Id, IocType = "Hash", Value = "c9e3f2a8d1b45678901234567890cdef67890123", Description = "Data exfiltration tool hash", AddedAt = now.AddDays(-1).AddHours(-18) },

            // INC-005 IOCs
            new() { Id = Guid.NewGuid(), IncidentId = inc5.Id, IocType = "Hash", Value = "d2f4e8b1c9a36789012345678901bcde45678901", Description = "Emotet dropper MD5", AddedAt = now.AddDays(-4).AddHours(-23) },
            new() { Id = Guid.NewGuid(), IncidentId = inc5.Id, IocType = "IP", Value = "45.33.32.156", Description = "Emotet C2 server", AddedAt = now.AddDays(-4).AddHours(-22) },

            // INC-006 IOCs
            new() { Id = Guid.NewGuid(), IncidentId = inc6.Id, IocType = "Hash", Value = "e5a1c7f3d9b28901234567890123cdef89012345", Description = "Mimikatz variant hash", AddedAt = now.AddHours(-9) },
            new() { Id = Guid.NewGuid(), IncidentId = inc6.Id, IocType = "IP", Value = "10.30.1.103", Description = "Internal pivot source (dev-ws-03)", AddedAt = now.AddHours(-8) },
            new() { Id = Guid.NewGuid(), IncidentId = inc6.Id, IocType = "Domain", Value = "ci-update-cdn.net", Description = "Suspected C2 callback domain", AddedAt = now.AddHours(-7) },

            // INC-007 IOCs
            new() { Id = Guid.NewGuid(), IncidentId = inc7.Id, IocType = "Hash", Value = "f6b2d8e4c1a79012345678901234def0901234ab", Description = "USB write utility found on workstation", AddedAt = now.AddDays(-3).AddHours(2) },
            new() { Id = Guid.NewGuid(), IncidentId = inc7.Id, IocType = "Email", Value = "j.thompson@personal-email.net", Description = "Personal email used to forward documents", AddedAt = now.AddDays(-2).AddHours(-18) },

            // INC-008 IOCs
            new() { Id = Guid.NewGuid(), IncidentId = inc8.Id, IocType = "IP", Value = "198.51.100.0/24", Description = "Primary DDoS botnet range", AddedAt = now.AddDays(-7).AddHours(1) },
            new() { Id = Guid.NewGuid(), IncidentId = inc8.Id, IocType = "IP", Value = "203.0.113.128/25", Description = "Secondary attack source range", AddedAt = now.AddDays(-7).AddHours(1) }
        };
        db.IOCs.AddRange(iocs);

        await db.SaveChangesAsync();

        // Seed Cases (only if not already seeded)
        if (!db.Cases.Any())
        {
            var case1 = new Case
            {
                Id = Guid.NewGuid(),
                CaseNumber = "CASE-2024-001",
                Title = "APT Campaign Analysis",
                Description = "Investigation into a sustained Advanced Persistent Threat campaign targeting critical infrastructure. Multiple incidents linked across departments.",
                Status = "InProgress",
                AssignedTo = "analyst1",
                CreatedBy = "admin",
                CreatedAt = now.AddDays(-10),
                UpdatedAt = now.AddHours(-1)
            };

            var case2 = new Case
            {
                Id = Guid.NewGuid(),
                CaseNumber = "CASE-2024-002",
                Title = "Finance Department Compromise",
                Description = "Coordinated attack targeting finance department systems. Evidence of credential theft and data exfiltration across multiple vectors.",
                Status = "Open",
                AssignedTo = "manager1",
                CreatedBy = "manager1",
                CreatedAt = now.AddDays(-5),
                UpdatedAt = now.AddDays(-1)
            };

            var case3 = new Case
            {
                Id = Guid.NewGuid(),
                CaseNumber = "CASE-2024-003",
                Title = "Malware Outbreak Investigation",
                Description = "Comprehensive investigation into the Emotet malware outbreak and DDoS attack. All affected systems remediated and lessons learned documented.",
                Status = "Closed",
                AssignedTo = "analyst2",
                CreatedBy = "analyst2",
                CreatedAt = now.AddDays(-15),
                UpdatedAt = now.AddDays(-3)
            };

            db.Cases.AddRange(case1, case2, case3);
            await db.SaveChangesAsync();

            // Link cases to incidents
            var incidentsByNumber = db.Incidents.ToDictionary(i => i.IncidentNumber);

            var caseLinks = new List<CaseIncident>();

            // CASE-001: linked to INC-001, INC-003, INC-006
            foreach (var num in new[] { "INC-2024-001", "INC-2024-003", "INC-2024-006" })
            {
                if (incidentsByNumber.TryGetValue(num, out var inc))
                    caseLinks.Add(new CaseIncident { CaseId = case1.Id, IncidentId = inc.Id, LinkedAt = now.AddDays(-9) });
            }

            // CASE-002: linked to INC-002, INC-004
            foreach (var num in new[] { "INC-2024-002", "INC-2024-004" })
            {
                if (incidentsByNumber.TryGetValue(num, out var inc))
                    caseLinks.Add(new CaseIncident { CaseId = case2.Id, IncidentId = inc.Id, LinkedAt = now.AddDays(-4) });
            }

            // CASE-003: linked to INC-005, INC-008
            foreach (var num in new[] { "INC-2024-005", "INC-2024-008" })
            {
                if (incidentsByNumber.TryGetValue(num, out var inc))
                    caseLinks.Add(new CaseIncident { CaseId = case3.Id, IncidentId = inc.Id, LinkedAt = now.AddDays(-14) });
            }

            db.CaseIncidents.AddRange(caseLinks);
            await db.SaveChangesAsync();
        }

        // Seed Alerts (only if not already seeded)
        if (!db.Alerts.Any())
        {
            var alerts = new List<Alert>
            {
                new Alert
                {
                    Id = Guid.NewGuid(),
                    AlertNumber = "ALT-2024-001",
                    Title = "Suspicious Outbound DNS",
                    Description = "Unusual DNS queries detected to known DGA domains. High volume of NX responses from endpoint 10.20.1.55. Possible C2 beaconing over DNS.",
                    Source = "SIEM",
                    Severity = "High",
                    Status = "New",
                    ReceivedAt = now.AddMinutes(-15)
                },
                new Alert
                {
                    Id = Guid.NewGuid(),
                    AlertNumber = "ALT-2024-002",
                    Title = "Failed Login Burst - admin",
                    Description = "147 failed login attempts for the admin account detected over 5 minutes from IP 91.243.80.12. Potential brute force attack in progress.",
                    Source = "Firewall",
                    Severity = "Critical",
                    Status = "New",
                    ReceivedAt = now.AddMinutes(-32)
                },
                new Alert
                {
                    Id = Guid.NewGuid(),
                    AlertNumber = "ALT-2024-003",
                    Title = "Port Scan Detected",
                    Description = "Comprehensive SYN scan detected from 203.78.192.44 targeting ports 22, 80, 443, 3389, 8080, 8443. 345 ports scanned in 60 seconds.",
                    Source = "IDS",
                    Severity = "Medium",
                    Status = "Acknowledged",
                    ReceivedAt = now.AddHours(-2),
                    AcknowledgedAt = now.AddHours(-1.5),
                    AcknowledgedBy = "analyst1"
                },
                new Alert
                {
                    Id = Guid.NewGuid(),
                    AlertNumber = "ALT-2024-004",
                    Title = "Malware Hash Match",
                    Description = "File hash a1b2c3d4e5f6789012345678901234567890abcd matched known Emotet dropper in threat intelligence feed. File found on endpoint bldga-ws-03.",
                    Source = "Endpoint",
                    Severity = "Critical",
                    Status = "Converted",
                    ReceivedAt = now.AddDays(-5).AddHours(-1)
                },
                new Alert
                {
                    Id = Guid.NewGuid(),
                    AlertNumber = "ALT-2024-005",
                    Title = "Unusual Data Transfer",
                    Description = "4.8GB outbound data transfer to external IP 185.199.110.153 detected over 2 hours from hr-db-01. Transfer volume 300x above baseline for this host.",
                    Source = "SIEM",
                    Severity = "High",
                    Status = "New",
                    ReceivedAt = now.AddMinutes(-8)
                },
                new Alert
                {
                    Id = Guid.NewGuid(),
                    AlertNumber = "ALT-2024-006",
                    Title = "Brute Force Attempt",
                    Description = "SSH brute force attempt detected from 45.128.96.55 targeting 3 servers. Attack subsided after 15 minutes with no successful logins.",
                    Source = "Firewall",
                    Severity = "Medium",
                    Status = "Dismissed",
                    ReceivedAt = now.AddHours(-6)
                },
                new Alert
                {
                    Id = Guid.NewGuid(),
                    AlertNumber = "ALT-2024-007",
                    Title = "C2 Beacon Detected",
                    Description = "Periodic beaconing to IP 45.33.32.156 every 60 seconds detected from dev-ws-05. Pattern consistent with known Cobalt Strike beacon profile.",
                    Source = "IDS",
                    Severity = "Critical",
                    Status = "New",
                    ReceivedAt = now.AddMinutes(-45)
                },
                new Alert
                {
                    Id = Guid.NewGuid(),
                    AlertNumber = "ALT-2024-008",
                    Title = "Privilege Escalation Alert",
                    Description = "User j.smith executed sudo with NOPASSWD to gain root access on fin-srv-02 at 02:34 UTC. This is outside normal working hours and inconsistent with role.",
                    Source = "SIEM",
                    Severity = "High",
                    Status = "Acknowledged",
                    ReceivedAt = now.AddHours(-4),
                    AcknowledgedAt = now.AddHours(-3.5),
                    AcknowledgedBy = "analyst2"
                }
            };

            db.Alerts.AddRange(alerts);
            await db.SaveChangesAsync();
        }
    }
}
