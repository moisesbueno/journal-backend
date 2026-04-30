
INSERT INTO qualis
            (id,description)
            SELECT UUID(),i.qualis_2019
FROM   importacao AS i
WHERE  qualis_2019 != ''
GROUP  BY i.qualis_2019;

