import { env } from "#/env";
import { createFileRoute } from "@tanstack/react-router";
import { createServerFn } from "@tanstack/react-start";

export const Route = createFileRoute("/")({
    loader: async () => {
        return await getEnv();
    },
    component: Home,
});

function Home() {
    const data = Route.useLoaderData();
    return (
        <div className="p-8">
            <h1 className="text-4xl font-bold">Welcome to TanStack Start</h1>
            <p className="mt-4 text-lg">
                Edit <code>src/routes/index.tsx</code> to get started.
            </p>
            <p className="mt-4 text-lg">
                The server url is <code>{data.serverUrl}</code>
            </p>
        </div>
    );
}

const getEnv = createServerFn().handler(() => {
    const serverUrl = env.SERVER_URL;
    console.log(serverUrl);
    return { serverUrl };
});
