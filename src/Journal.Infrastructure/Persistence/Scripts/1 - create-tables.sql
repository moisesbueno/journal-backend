CREATE TABLE importacao
  (
     issn        VARCHAR(20),
     name        VARCHAR(255),
     qualis_2019 VARCHAR(10),
     CreatedAt DateTime NOT NULL DEFAULT CURRENT_TIMESTAMP,
     UpdatedAt DateTime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
  );

CREATE TABLE database_indexation
  (
     Id          CHAR(36),
     description VARCHAR(50),
     CreatedAt DateTime NOT NULL DEFAULT CURRENT_TIMESTAMP,
     UpdatedAt DateTime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
     PRIMARY KEY(id)
  );

CREATE TABLE qualis
  (
     Id          CHAR(36),
     description VARCHAR(10),
     CreatedAt DateTime NOT NULL DEFAULT CURRENT_TIMESTAMP,
     UpdatedAt DateTime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
     PRIMARY KEY(id)
  );

CREATE TABLE language
  (
     Id          CHAR(36),
     description VARCHAR(50),
     CreatedAt DateTime NOT NULL DEFAULT CURRENT_TIMESTAMP,
     UpdatedAt DateTime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
     PRIMARY KEY(id)
  );

CREATE TABLE format
  (
     Id          CHAR(36),
     maxpages INT,
     maxwords INT,
     space    INT,
     fontsize INT,
     CreatedAt DateTime NOT NULL DEFAULT CURRENT_TIMESTAMP,
     UpdatedAt DateTime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
     PRIMARY KEY(id)
  );

CREATE TABLE journal
  (
     Id       CHAR(36),
     issn     VARCHAR(20),
     name     VARCHAR(255),
     qualisid CHAR(36),
     aimscope VARCHAR(255),
     formatid CHAR(36),
     apc      BOOLEAN,
     url      VARCHAR(200),
     CreatedAt DateTime NOT NULL DEFAULT CURRENT_TIMESTAMP,
     UpdatedAt DateTime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
     PRIMARY KEY(Id),
     FOREIGN KEY(qualisid) REFERENCES qualis(Id),
     FOREIGN KEY(formatid) REFERENCES format(Id)
  );

CREATE TABLE journal_language
  (
     Journalid  CHAR(36),
     Languageid CHAR(36),
     CreatedAt DateTime NOT NULL DEFAULT CURRENT_TIMESTAMP,
     UpdatedAt DateTime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
     FOREIGN KEY(journalid) REFERENCES journal(id),
     FOREIGN KEY(languageid) REFERENCES language(id)
  );

CREATE TABLE journal_indexation
  (
     Journalid           CHAR(36),
     Journalindexationid CHAR(36),
     CreatedAt DateTime NOT NULL DEFAULT CURRENT_TIMESTAMP,
     UpdatedAt DateTime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
     FOREIGN KEY(journalid) REFERENCES journal(id),
     FOREIGN KEY(journalindexationid) REFERENCES database_indexation(id)
  );

CREATE TABLE user
(
	Id CHAR(36) NOT NULL,
    Email varchar(80) NOT NULL,
    Password varchar(80) NOT NULL,
    CreatedAt DateTime NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DateTime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    primary key (Id)
);