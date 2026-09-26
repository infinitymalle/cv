import { ProjectList } from "../features/projects/ProjectList";

export function App() {
  return (
    <div className="page">
      <header className="page-header">
        <h1>Malkolm Lundkvist</h1>
        <p className="tagline">Computer Science and Engineering student</p>
      </header>

      <main>
        <section aria-labelledby="projects-heading">
          <h2 id="projects-heading">Projects</h2>
          <ProjectList />
        </section>
      </main>
    </div>
  );
}
