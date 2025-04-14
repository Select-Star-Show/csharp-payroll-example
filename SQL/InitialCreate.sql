CREATE TABLE employees (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    name text NOT NULL,
    role text NOT NULL,
    CONSTRAINT "PK_employees" PRIMARY KEY (id)
);

